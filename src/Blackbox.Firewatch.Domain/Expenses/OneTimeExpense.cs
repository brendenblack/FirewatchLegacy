using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain.Expenses
{
    public class OneTimeExpense : Expense
    {
        public OneTimeExpense()
        {
        }

        public override string ExpenseType => "one time";

    }
}
