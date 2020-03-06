using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions
{
    public class AddTransactionsResponse
    {
        public List<int> CreatedIds { get; set; } = new List<int>();
    }
}