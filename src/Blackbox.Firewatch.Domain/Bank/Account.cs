using System;
using System.Collections.Generic;

namespace Blackbox.Firewatch.Domain.Bank
{
    public class Account
    {
        /// <summary>
        /// Internal ID for this particular record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A value unique to the particular <see cref="FinancialInstitution"/> that identifies this account.
        /// <para>
        /// This number is not guaranteed to be unique globally within this system. Use <see cref="Id"/> for that.
        /// </para>
        /// </summary>
        public string AccountNumber { get; set; }

        public string AccountType { get; set; }

        public string OwnerId { get; set; }
        public Person Owner { get; set; }

        /// <summary>
        /// What financial institution this account belongs to.
        /// </summary>
        public FinancialInstitution Institution { get; set; } = FinancialInstitution.Empty;

        public string Nickname { get; set; }

        public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    }
}
