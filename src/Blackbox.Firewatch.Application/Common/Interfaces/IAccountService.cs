using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Common.Interfaces
{
    /// <summary>
    /// A service for accessing metadata about credit and debit cards.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Looks up metadata for the provided card number based on the first 4 to 6 digits.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public Task LookupByNumber(string cardNumber);

        public Account CreateAccountByNumber(Person owner, string accountNumber);
    }
}
