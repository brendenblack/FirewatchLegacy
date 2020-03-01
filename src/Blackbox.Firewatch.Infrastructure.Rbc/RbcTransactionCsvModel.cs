using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Rbc
{
    public class RbcTransactionCsvModel
    {
        public string AccountType { get; set; }

        public string AccountNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public string ChequeNumber { get; set; }

        public string Description1 { get; set; }

        public string Description2 { get; set; }

        public decimal? CadAmount { get; set; } = 0;

        public decimal? UsdAmount { get; set; } = 0;
    }
}
