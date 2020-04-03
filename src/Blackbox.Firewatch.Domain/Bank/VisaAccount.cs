using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain.Bank
{
    public class VisaAccount : Account
    {
        private VisaAccount() 
        {
            AccountType = AccountTypes.VISA;
        }
        
        public VisaAccount(Person owner, string number)
        {
            Owner = owner;
            OwnerId = owner.Id;
            AccountNumber = number;
            DisplayName = MaskAccountNumber(number);
            AccountType = AccountTypes.VISA;
        }



        public static string MaskAccountNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException();
            }

            number = number.Trim().Replace(" ", "");

            if (number.Length != 16)
            {
                throw new ArgumentException("The provided account number {} is not a recognized Visa number", number);
            }

            // TODO: mask
            var maskBuilder = new StringBuilder(number)
                .Remove(4, 8)
                .Insert(4, " **** **** ");

            return maskBuilder.ToString();
        }
    }
}
