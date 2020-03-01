using System;
using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions
{
    public class TransactionModel
    {
        public string AccountId { get; set; }

        public DateTime Date { get; set; }

        public IReadOnlyCollection<string> Descriptions { get; set; } = new List<string>();

        public List<string> Tags { get; set; } = new List<string>();

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string AccountNumber { get; set; }
    }
}