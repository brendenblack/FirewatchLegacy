using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv
{
    public class ParseCsvResponse
    {
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}