using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain.Bank
{
    public enum TransactionTypes
    {
        /// <summary>
        /// A generic credit (positive value) transaction.
        /// </summary>
        CREDIT,
        /// <summary>
        /// A generic debit (negative value) transaction.
        /// </summary>
        DEBIT,
        /// <summary>
        /// A debit where the account owner transferred to cash.
        /// </summary>
        CASH_WITHDRAWAL,
        VOID,
        AUTHORIZATION,
        CAPTURE,
        /// <summary>
        /// A credit that is the result of a transfer. Generally paired with <see cref="TRANSFER_OUT"/> from a different account.
        /// </summary>
        TRANSFER_IN,
        /// <summary>
        /// A debit that is the result of a transfer. Generally paired with <see cref="TRANSFER_IN"/> from a different account.
        /// </summary>
        TRANSFER_OUT,
        /// <summary>
        /// A debit that is being assessed as a result of fees.
        /// </summary>
        DEBIT_FEES,
        /// <summary>
        /// A debit that is being applied specifically to an interesting payment.
        /// </summary>
        DEBIT_INTEREST_PAYMENT,
        /// <summary>
        /// A debit that is being applied specifically to the principal of a loan.
        /// </summary>
        DEBIT_PRINCIPAL_PAYMENT,
        /// <summary>
        /// A catch all when no other type is suitable.
        /// </summary>
        OTHER
    }
    // fees, withdrawals, principal payments, interest payments, deposits, etc...
}
