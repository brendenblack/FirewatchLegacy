using Blackbox.Firewatch.Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain.Expenses
{
    public abstract class Expense
    {
        public int Id { get; set; }
        public abstract string ExpenseType { get; } 
        public ICollection<Transaction> AssociatedTransactions { get; } = new List<Transaction>();
    }
}
