using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TeixeiraSoftware.Finance;
using Xunit;
using Xunit.Abstractions;

namespace Blackbox.Firewatch.Infrastructure.Rbc.IntegrationTests.RbcTransactionImporterTests
{
    public class RbcTransactionParserTests
    {
        private readonly ITestOutputHelper output;

        public RbcTransactionParserTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        public string ReadLocalTestFile(string filename)
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(dirName, filename);

            string contents = File.ReadAllText(file);
            return contents;
        }

        [Fact]
        public async Task ParseTransactionsFromCsv_ReadWellFormedFile()
        {
            var testContents = ReadLocalTestFile("test1.csv");
            var logger = new XUnitLogger<RbcTransactionParser>(this.output);
            
            var sut = new RbcTransactionParser(logger);

            var transactions = await sut.ParseTransactionsFromCsv(testContents, "CAD");

            transactions.Count.ShouldBe(23);
        }

        [Theory]
        [InlineData(2019, 11, 15, 3085.92, "CAD")]
        public async Task CreateExpectedTransactions(int year, int month, int day, decimal expectedAmount, string expectedCurrency)
        {
            var date = new DateTime(year, month, day);
            var logger = new XUnitLogger<RbcTransactionParser>(this.output);
            var testContents = ReadLocalTestFile("test1.csv");
            var sut = new RbcTransactionParser(logger);

            var transactions = await sut.ParseTransactionsFromCsv(testContents, "CAD");

            var transaction = transactions.FirstOrDefault(tx => tx.Date == date);
            transaction.ShouldNotBeNull("Expected test entry was not found. This suggests that the test file has changed and this method must be updated.");

            transaction.Amount.ShouldBe(expectedAmount);
            transaction.Currency.ShouldBe(Currency.ByAlphabeticCode(expectedCurrency));
        }
    }
}
