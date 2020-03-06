using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchTransactions
{
    public class FetchTransactionsResponse
    {
        public List<FetchTransactionModel> Transactions { get; set; } = new List<FetchTransactionModel>();
    }
}