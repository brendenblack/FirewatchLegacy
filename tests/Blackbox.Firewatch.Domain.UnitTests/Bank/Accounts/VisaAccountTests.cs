using Blackbox.Firewatch.Domain.Bank;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Blackbox.Firewatch.Domain.UnitTests.Bank.Accounts
{
    public class VisaAccountTests
    {
        [Theory]
        [InlineData("4500123457890123", "4500 **** **** 0123")]
        [InlineData("4500 1234 5789 0123", "4500 **** **** 0123")]
        [InlineData("4500123457890123    ", "4500 **** **** 0123")]
        [InlineData("    4500123457890123", "4500 **** **** 0123")]
        [InlineData("4 5 0 0 1 2 3 4 5 7 8 9 0 1 2 3", "4500 **** **** 0123")]
        [InlineData("4500********0123", "4500 **** **** 0123")]
        [InlineData("4500 **** **** 0123", "4500 **** **** 0123")]
        public void MaskAccountNumber_ShouldMaskMiddleDigits(string input, string expected)
        {
            VisaAccount.MaskAccountNumber(input).ShouldBe(expected);
        }
    }
}
