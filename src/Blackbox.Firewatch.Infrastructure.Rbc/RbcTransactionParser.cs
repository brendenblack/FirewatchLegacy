using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Domain.Bank;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Infrastructure.Rbc
{
    public class RbcTransactionParser : ITransactionParser
    {
        private readonly ILogger<RbcTransactionParser> logger;

        public RbcTransactionParser(ILogger<RbcTransactionParser> logger)
        {
            this.logger = logger;
        }

        public FinancialInstitution SupportedInstitution => FinancialInstitution.RoyalBank;

#pragma warning disable 1998
        public async Task<IReadOnlyCollection<Transaction>> ParseTransactionsFromCsv(string contents, string defaultCurrency)
        {
            var transactions = new List<Transaction>();

            using (var reader = new StringReader(contents))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<RbcTransactionCsvMap>();
                var records = csv.GetRecords<RbcTransactionCsvModel>()
                    .ToList();

                var accounts = records
                    .Select(r => new { r.AccountNumber, r.AccountType })
                    .Distinct()
                    .Select(r => new Account()
                    {
                        Institution = FinancialInstitution.RoyalBank,
                        AccountNumber = r.AccountNumber
                    })
                    .ToList();

                logger.LogTrace("Found {} distinct accounts in provided csv contents.",
                    accounts.Count);

                foreach (var record in records)
                {
                    var account = accounts.First(a => a.AccountNumber == record.AccountNumber);
                    string currency;
                    decimal amount;
                    if (record.CadAmount.HasValue)
                    {
                        currency = "CAD";
                        amount = record.CadAmount.Value;
                    }
                    else if (record.UsdAmount.HasValue)
                    {
                        currency = "USD";
                        amount = record.UsdAmount.Value;
                    }
                    else
                    {
                        logger.LogWarning("Unable to process transaction because it had no amount: {}", record.ToString());
                        continue;
                    }
                    var transaction = new Transaction(account, record.TransactionDate, amount, currency);
                    if (!string.IsNullOrWhiteSpace(record.Description1))
                    {
                        transaction.Descriptions.Add(record.Description1.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(record.Description2))
                    {
                        transaction.Descriptions.Add(record.Description2.Trim());
                    }

                    transactions.Add(transaction);
                }

            }

            return transactions;
        }
#pragma warning restore 1998
    }
}
