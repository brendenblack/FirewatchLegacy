using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain.Expenses
{
    public class OngoingExpense : Expense
    {
        //public static string ExpenseType => "ongoing";

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public override string ExpenseType => "ongoing";
    }
}
