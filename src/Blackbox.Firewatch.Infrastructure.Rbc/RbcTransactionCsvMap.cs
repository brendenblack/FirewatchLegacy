using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Rbc
{
    public class RbcTransactionCsvMap : ClassMap<RbcTransactionCsvModel>
    {
        public RbcTransactionCsvMap()
        {
            Map(m => m.AccountType).Index(0);
            Map(m => m.AccountNumber).Index(1);
            Map(m => m.TransactionDate).Index(2);
            Map(m => m.ChequeNumber).Index(3);
            Map(m => m.Description1).Index(4);
            Map(m => m.Description2).Index(5);
            Map(m => m.CadAmount).Index(6);
            Map(m => m.UsdAmount).Index(7);
        }
    }
}
