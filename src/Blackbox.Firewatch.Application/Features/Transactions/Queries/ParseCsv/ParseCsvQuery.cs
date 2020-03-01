using AutoMapper;
using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Security;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv
{
    public class ParseCsvQuery : PersonScopedAuthorizationRequiredRequest, IRequest<ParseCsvResponse>
    {
        public override string[] AuthorizedRoles => new string[] { Roles.Administrator };

        public string FinancialInstitutionAbbreviation { get; set; }

        public string CsvContent { get; set; }

        /// <summary>
        /// Whether or not the system should attempt to identify parsed transactions as potential duplicates
        /// from ones already stored in the system.
        /// </summary>
        public bool ShouldIdentifyDuplicates { get; set; }

        public class ParseCsvHandler : IRequestHandler<ParseCsvQuery, ParseCsvResponse>
        {
            private readonly ILogger<ParseCsvHandler> logger;
            private readonly IBankContext bankContext;
            private readonly IEnumerable<ITransactionParser> transactionParsers;
            private readonly IMapper mapper;

            public ParseCsvHandler(
                ILogger<ParseCsvHandler> logger, 
                IBankContext bankContext,
                IEnumerable<ITransactionParser> transactionParsers,
                IMapper mapper)
            {
                this.logger = logger;
                this.bankContext = bankContext;
                this.transactionParsers = transactionParsers;
                this.mapper = mapper;
            }

            public async Task<ParseCsvResponse> Handle(ParseCsvQuery request, CancellationToken cancellationToken)
            {
                // use #First() instead of #FirstOrDefault here because we are validating the request
                // with FluentValidation so if we got to this point & and this still fails, it is an 
                // exceptional circumstance.
                // see ParseCsvQueryValidator.cs
                var parser = transactionParsers.First(p => p.SupportedInstitution.Abbreviation == request.FinancialInstitutionAbbreviation);
                logger.LogDebug("Attempting to parse provided CSV content using {}", parser.GetType().FullName);
                var parsedTransactions = await parser.ParseTransactionsFromCsv(request.CsvContent);

                var transactionModels = this.mapper.Map<List<TransactionModel>>(parsedTransactions);

                if (request.ShouldIdentifyDuplicates)
                {
                    logger.LogDebug("Attempting to identify duplicates in provided transaction log for user {}", 
                        request.OwnerId);

                    var firstTransaction = transactionModels
                        .Select(t => t.Date)
                        .Min();

                    var lastTransaction = transactionModels
                        .Select(t => t.Date)
                        .Max();

                    var accountsInPlay = parsedTransactions
                        .Select(t => t.Account.AccountNumber)
                        .Distinct();

                    // Limit the universe of transactions as much as possible to speed up the 
                    // identification process.
                    // First we filter by a date range, then we filter out transactions from
                    // accounts that aren't involved in this query.
                    // We check the accountsInPlay array as the master because not all accounts 
                    // represented here have to already exist.
                    var existingTransactions = this.bankContext.Transactions
                        .Where(t => t.Account.OwnerId == request.OwnerId)
                        .Where(t => t.Date >= firstTransaction)
                        .Where(t => t.Date <= lastTransaction)
                        .Where(t => accountsInPlay.Contains(t.Account.AccountNumber))
                        .ToList();

                    transactionModels = IdentifyDuplicates(transactionModels, existingTransactions);
                }

                return new ParseCsvResponse { Transactions = transactionModels };

            }

            public List<TransactionModel> IdentifyDuplicates(
                IEnumerable<TransactionModel> suppliedTransactions,
                IEnumerable<Transaction> authoritativeTransactions)
            {
                var scannedTransactions = new List<TransactionModel>();
                
                foreach (var transaction in suppliedTransactions)
                {
                    var duplicateIds = authoritativeTransactions
                        .Where(t => t.Date == transaction.Date)
                        .Where(t => t.Amount == transaction.Amount)
                        .Where(t => t.Currency.AlphabeticCode.Equals(transaction.Currency, StringComparison.OrdinalIgnoreCase))
                        .Select(t => t.Id)
                        .ToList();

                    var newTransaction = new TransactionModel
                    {
                        AccountNumber = transaction.AccountNumber,
                        Amount = transaction.Amount,
                        Currency = transaction.Currency,
                        Date = transaction.Date,
                        Descriptions = transaction.Descriptions,
                        DuplicateIds = duplicateIds
                    };

                    scannedTransactions.Add(newTransaction);
                }

                return scannedTransactions;
            }
        }
    }
}
