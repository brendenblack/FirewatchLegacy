using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeixeiraSoftware.Finance;

namespace Blackbox.Firewatch.Domain.Bank
{
    public class Transaction : AuditableEntity
    {
        [Obsolete("The default constructor is only a conceit for easier integration with Automapper and EF.")]
        public Transaction() { }

        public Transaction(Account account, DateTime date, decimal amount, string currencyCode = "CAD")
        {
            Currency = Currency.ByAlphabeticCode(currencyCode);
            this.Account = account;
            this.Date = date;
            this.Amount = amount;
            this.TransactionType = amount >= 0 ? TransactionTypes.CREDIT : TransactionTypes.DEBIT;

            // Force generation of hash code so it can be saved 
            //var hashCode = -1346463035;
            //hashCode = hashCode * -1521134295 + Date.GetHashCode();
            //hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Payee);
            //hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            //this.HashCode = hashCode;
        }

        /// <summary>
        /// Uniquely identify this transaction within this system. This id has no relation to 
        /// any ids assigned by the providing financial institution.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User-defined metadata about this transactions.
        /// </summary>
        public ICollection<string> Tags { get; } = new List<string>();

        /// <summary>
        /// User defined notes about this transaction.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// What account this transaction is attributed to.
        /// </summary>
        public Account Account { get; private set; }

        /// <summary>
        /// When this transaction occurred.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Any notes supplied by the financial institution.
        /// </summary>
        public IList<string> Descriptions { get; set; } = new List<string>();

        /// <summary>
        /// The value of this transaction, agnostic of the currency.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// The currency this transaction took plac ein.
        /// </summary>
        public Currency Currency { get; private set; }

        public TransactionTypes TransactionType { get; set; }

        public static TransactionTypes[] CreditTransactionTypes { get; } = new TransactionTypes[]
        {
            TransactionTypes.CREDIT,
            TransactionTypes.TRANSFER_IN,
        };

        public static TransactionTypes[] DebitTransactionTypes { get; } = new TransactionTypes[]
        {
            TransactionTypes.DEBIT,
            TransactionTypes.CASH_WITHDRAWAL,
            TransactionTypes.TRANSFER_OUT,
            TransactionTypes.DEBIT_FEES,
            TransactionTypes.DEBIT_INTEREST_PAYMENT,
            TransactionTypes.DEBIT_PRINCIPAL_PAYMENT
        };

        public bool IsCredit => CreditTransactionTypes.Contains(TransactionType);
        public bool IsDebit => DebitTransactionTypes.Contains(TransactionType);

        public int HashCode { get; private set; }
    }
}
