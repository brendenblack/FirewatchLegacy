using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchUserTransactions
{
    public class FetchUserTransactionsResponse
    {
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}