using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Security;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions
{
    public class AddTransactionsCommand : IRequest<Result<AddTransactionsResponse>>
    {
        public List<AddTransactionModel> Transactions { get; set; } = new List<AddTransactionModel>();

        public string PersonId { get; set; }

        public class AddTransactionsHandler : IRequestHandler<AddTransactionsCommand, Result<AddTransactionsResponse>>
        {
            private readonly ILogger<AddTransactionsHandler> logger;
            private readonly IBankContext bankContext;

            public AddTransactionsHandler(
                ILogger<AddTransactionsHandler> logger,
                IBankContext bankContext)
            {
                this.logger = logger;
                this.bankContext = bankContext;
            }

            public async Task<Result<AddTransactionsResponse>> Handle(AddTransactionsCommand request, CancellationToken cancellationToken)
            {
                // Get the list of accounts owned by the owner identified in the request.
                var accounts = await bankContext.Accounts
                    .Where(a => a.OwnerId == request.PersonId)
                    .ToListAsync();

                var accountGroupedTransactions = request.Transactions
                    .GroupBy(tx => tx.AccountNumber);

                var addedTransactions = new List<Transaction>();

                foreach (var accountGroup in accountGroupedTransactions)
                {
                    var account = accounts
                        .Where(a => a.AccountNumber == accountGroup.Key)
                        .FirstOrDefault();

                    if (account == null)
                    {
                        account = CreateAccount(request.PersonId, accountGroup.Key);
                        bankContext.Accounts.Add(account);
                        await bankContext.SaveChangesAsync();
                        //logger.LogDebug("Skipping adding {} transactions in to account {} because it does not exist.",
                        //    accountGroup.Count(),
                        //    accountGroup.Key);
                        //continue;
                    }

                    foreach (var tx in accountGroup)
                    {
                        var transaction = new Transaction(account, tx.Date, tx.Amount, tx.Currency)
                        {
                            Descriptions = tx.Descriptions
                        };
                        bankContext.Transactions.Add(transaction);
                        addedTransactions.Add(transaction);
                    }

                    await bankContext.SaveChangesAsync();
                }

                var response = new AddTransactionsResponse
                {
                    CreatedIds = addedTransactions.Select(tx => tx.Id).ToList()
                };

                return Result.Ok(response);
            }

            public Account CreateAccount(string ownerId, string accountNumber)
            {
                var account = new Account()
                {
                    OwnerId = ownerId,
                    AccountNumber = accountNumber
                };

                return account;
            }
        }
    }
}
