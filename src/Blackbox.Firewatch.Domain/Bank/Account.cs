using System;
using System.Collections.Generic;
using TeixeiraSoftware.Finance;

namespace Blackbox.Firewatch.Domain.Bank
{
    public class Account : AuditableEntity
    {
        /// <summary>
        /// Internal ID for this particular record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A value unique to the particular <see cref="FinancialInstitution"/> that identifies this account.
        /// <para>
        /// This number is not guaranteed to be unique globally within this system; use <see cref="Id"/> for that.
        /// </para>
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// A user-friendly name for this account.
        /// </summary>
        public string DisplayName { get; set; }

        public AccountTypes AccountType { get; protected set; } = AccountTypes.OTHER;

        public string OwnerId { get; set; }
        public Person Owner { get; set; }

        /// <summary>
        /// What financial institution this account belongs to.
        /// </summary>
        public FinancialInstitution Institution { get; set; } = FinancialInstitution.Empty;

        public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();


        public void AddTransaction(DateTime date, string[] descriptions, decimal amount, string currencyCode)
        {
            var transaction = new Transaction(this, date, amount, currencyCode);
            this.Transactions.Add(transaction);
        }

        public static string FormatAccountNumber(string accountType, string accountNumber)
        {
            if (accountType.ToUpper() == "VISA")
            {
                return "";
            } 
            else
            {
                return accountNumber;
            }
        }

        public static Account FromNumber(Person owner, string accountNumber)
        {
            var account = new Account()
            {
                Owner = owner,
                OwnerId = owner.Id
            };


            return account;
        }


    }
}
