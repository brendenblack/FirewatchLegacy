using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Common.Interfaces
{
    public interface ITransactionParser
    {
        /// <summary>
        /// What institution's CSV format this parser understands.
        /// </summary>
        FinancialInstitution SupportedInstitution { get; }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="defaultCurrency">What currency to assume if the transaction record does not specify one.</param>
        /// <returns></returns>
        public Task<IReadOnlyCollection<Transaction>> ParseTransactionsFromCsv(string contents, string defaultCurrency = "CAD");
    }
}
