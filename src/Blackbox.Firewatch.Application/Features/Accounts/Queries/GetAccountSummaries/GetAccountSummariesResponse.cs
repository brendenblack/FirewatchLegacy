using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Accounts.Queries.GetAccountSummaries
{
    public class GetAccountSummariesResponse
    {
        public List<AccountSummaryModel> Accounts = new List<AccountSummaryModel>();
    }
}