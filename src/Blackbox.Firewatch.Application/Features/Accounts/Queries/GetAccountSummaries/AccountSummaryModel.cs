using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Accounts.Queries.GetAccountSummaries
{
    public class AccountSummaryModel
    {
        public string AccountNumber { get; set; }

        public string FinancialInstitution { get; set; }

        public int Transactions { get; set; }

        public DateTime EarliestTransaction { get; set; }

        public DateTime LatestTransaction { get; set; }

        public double Total { get; set; }
    }
}
