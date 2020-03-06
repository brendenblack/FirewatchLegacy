using System;
using System.Collections.Generic;

namespace Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions
{
    public class AddTransactionModel
    {
        public string AccountNumber { get; set; }

        public DateTime Date { get; set; }

        public List<string> Descriptions { get; set; } = new List<string>();

        public List<string> Tags { get; set; } = new List<string>();

        public decimal Amount { get; set; }

        public string Currency { get; set; }

    }
}