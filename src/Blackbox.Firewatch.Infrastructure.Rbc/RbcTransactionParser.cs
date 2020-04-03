using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv;
using Blackbox.Firewatch.Domain;
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
        public async Task<IReadOnlyCollection<TransactionModel>> ParseTransactionsFromCsv(string contents, string defaultCurrency)
        {
            var transactions = new List<TransactionModel>();

            using (var reader = new StringReader(contents))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<RbcTransactionCsvMap>();
                csv.Configuration.BadDataFound = null;
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
                    string accountNumber = (record.AccountType.ToUpper() == "Visa")
                        ? record.AccountNumber
                        : "008" + record.AccountNumber;

                    var account = accounts.First(a => a.AccountNumber == accountNumber);
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

                    var transaction = new TransactionModel
                    {
                        AccountNumber = record.AccountNumber,
                        Amount = amount,
                        Currency = currency,
                        Date = record.TransactionDate
                    };

                    var descriptions = new List<string>();
                    if (!string.IsNullOrWhiteSpace(record.Description1))
                    {
                        descriptions.Add(record.Description1.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(record.Description2))
                    {
                        descriptions.Add(record.Description2.Trim());
                    }
                    transaction.Descriptions = descriptions;

                    transactions.Add(transaction);
                }

            }

            return transactions;
        }
#pragma warning restore 1998

        public IReadOnlyCollection<Account> DetermineAccounts(Person owner, IEnumerable<Account> ownerAccounts, IEnumerable<RbcTransactionCsvModel> records)
        {
           


            var accountRecords = records
                    .Select(r => new { r.AccountNumber, r.AccountType })
                    .Distinct();

            var accounts = new List<Account>();
            foreach (var accountRecord in accountRecords)
            {
                var accountType = DetermineAccountType(accountRecord.AccountType);
                //var accountNumber = DetermineAccountNumber(accountType, accountRecord.AccountNumber);
            }

            return accounts;
        }

        public AccountTypes DetermineAccountType(string accountType)
        {
            switch (accountType.ToUpper())
            {
                case "VISA":
                    return AccountTypes.VISA;
                case "CHEQUING":
                    return AccountTypes.CHEQUING;
                default:
                    return AccountTypes.OTHER;
            }
        }

        public string DetermineAccountTypeFromNumber(string accountNumber)
        {
            throw new NotImplementedException();

            // https://stevemorse.org/ssn/List_of_Bank_Identification_Numbers.html#Visa_.2845.2A.2A.2A.2A.29
            if (accountNumber.StartsWith("4510") || accountNumber.StartsWith("4512") || accountNumber.StartsWith("4515"))
            {
                return "Visa";
            }
            else if (accountNumber.StartsWith("4514"))
            {
                return "AVION Visa";
            }
            else
            {
                return "";
            }
        }
    }
}
