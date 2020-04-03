using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features
{
    /// <summary>
    /// A stubbed out service built on assumptions.
    /// </summary>
    public class DummyAccountService : IAccountService
    {
        private readonly IBankContext _bankContext;

        public DummyAccountService(IBankContext bankContext)
        {
            _bankContext = bankContext;
        }

        public async Task<Account> CreateAccountByNumber(Person owner, string accountNumber)
        {
            Account account = _bankContext.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber && a.OwnerId == owner.Id);
            
            if (account != null)
            {
                return account;
            }

            accountNumber = NormalizeAccountNumber(accountNumber);
            account.Institution = DetermineInstitutionFromAccountNumber(accountNumber);
            if (IsVisa(accountNumber))
            {
                account = new VisaAccount(owner, accountNumber);
            }
            else
            {
                account = new Account()
                {
                    Owner = owner,
                    OwnerId = owner.Id,
                    AccountNumber = accountNumber,
                };
            }

            _bankContext.Accounts.Add(account);
            await _bankContext.SaveChangesAsync();

            return account;
        }

        public string NormalizeAccountNumber(string accountNumber)
        {
            return accountNumber
                .Trim()
                .Replace(" ", "")
                .Replace("-", "");
        }

        public bool IsVisa(string accountNumber)
        {
            // https://en.wikipedia.org/wiki/Payment_card_number#Issuer_identification_number_.28IIN.29
            return (accountNumber.Length == 16 && accountNumber.StartsWith("4"));
        }


        public FinancialInstitution DetermineInstitutionFromAccountNumber(string accountNumber)
        {
            // https://support.kraken.com/hc/en-us/articles/115012377707-CAD-domestic-wire-withdrawals-How-to-look-up-your-Canadian-bank-account-information
            if (accountNumber.StartsWith("003")
                || accountNumber.StartsWith("4510")
                || accountNumber.StartsWith("4512")
                || accountNumber.StartsWith("4514")
                || accountNumber.StartsWith("4515")
                || accountNumber.StartsWith("4519"))
            {
                return FinancialInstitution.RoyalBank;
            }
            else if (accountNumber.StartsWith("001"))
            {
                return new FinancialInstitution()
                {
                    Abbreviation = "BMO",
                    Name = "Bank of Montreal"
                };
            }
            else
            {
                return FinancialInstitution.Empty;
            }
        }

        public Task LookupByNumber(string cardNumber)
        {
            throw new NotImplementedException();
        }
    }
}
