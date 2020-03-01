using AutoMapper;
using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv;
using Blackbox.Firewatch.Application.Infrastructure.Mapping;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Infrastructure.Persistence;
using FakeItEasy;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv.ParseCsvQuery;

namespace Blackbox.Firewatch.Application.UnitTests.Features.Transactions.Queries
{
    [Collection("QueryTests")]
    public class ParseCsvQueryTests
    {
        private readonly ITestOutputHelper _output;
        private readonly QueryTestFixture _fixture;

        public ParseCsvQueryTests(ITestOutputHelper output, QueryTestFixture fixture)
        {
            this._output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_ThrowsWhenParserNotFound()
        {
            var logger = new XUnitLogger<ParseCsvHandler>(_output);
            var bankContext = A.Fake<IBankContext>();
            var mapper = A.Fake<IMapper>();
            var parsers = new List<ITransactionParser>();
            var sut = new ParseCsvHandler(logger, bankContext, parsers, mapper);
            var request = new ParseCsvQuery
            {
                FinancialInstitutionAbbreviation = "RBC",
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(request, CancellationToken.None));
            exception.Message.ShouldBe("Sequence contains no matching element");
        }

        [Fact]
        public async Task Handle_ReturnsExpectedResults_WhenIdentifyDuplicatesIsFalse()
        {
            var bankContext = A.Fake<IBankContext>();
            var mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); }).CreateMapper();
            var logger = new XUnitLogger<ParseCsvHandler>(_output);
            var parser = A.Fake<ITransactionParser>();
            A.CallTo(() => parser.SupportedInstitution).Returns(FinancialInstitution.RoyalBank);
            var testAccount = new Account() { AccountNumber = "12345" };
            IReadOnlyCollection<Transaction> returnedTransactions = new List<Transaction>()
            {
                new Transaction(testAccount, DateTime.Now, 100, "CAD")
            };
            A.CallTo(() => parser.ParseTransactionsFromCsv("", "CAD")).Returns(Task.FromResult(returnedTransactions));
            var sut = new ParseCsvHandler(logger, bankContext, new List<ITransactionParser>() { parser }, mapper);
            var request = new ParseCsvQuery
            {
                FinancialInstitutionAbbreviation = "RBC",
                CsvContent = "", // mocked return from parser
                ShouldIdentifyDuplicates = false,
            };

            var response = await sut.Handle(request, CancellationToken.None);

            response.Transactions.Count.ShouldBe(1);
            var tx = response.Transactions.First();
            tx.AccountNumber.ShouldBe("12345");
            tx.Amount.ShouldBe(100);
        }

        [Fact]
        public async Task Handle_ShouldNotMarkDuplicates_WhenIdentifyDuplicatesIsFalse()
        {
            var testAccount = new Account() { AccountNumber = "12345" };
            IReadOnlyCollection<Transaction> returnedTransactions = new List<Transaction>()
            {
                new Transaction(testAccount, new DateTime(2020,01,10), 100),
                new Transaction(testAccount, new DateTime(2020,01,11), 120.4m),
                new Transaction(testAccount, new DateTime(2020,01,09), 12),
            };
            _fixture.Context.Transactions.AddRange(returnedTransactions);
            await _fixture.Context.SaveChangesAsync();
            var mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); }).CreateMapper();
            var logger = new XUnitLogger<ParseCsvHandler>(_output);
            var parser = A.Fake<ITransactionParser>();
            A.CallTo(() => parser.SupportedInstitution).Returns(FinancialInstitution.RoyalBank);
            var transactionModels = mapper.Map<List<TransactionModel>>(returnedTransactions);
            // When the handler tries to parse the csv content it will receive a list mapped from the same
            // transactions we added earlier, ensuring they appear as duplicates.
            A.CallTo(() => parser.ParseTransactionsFromCsv("", "CAD")).Returns(Task.FromResult(returnedTransactions));
            var sut = new ParseCsvHandler(logger, _fixture.Context, new List<ITransactionParser>() { parser }, mapper);
            var request = new ParseCsvQuery
            {
                FinancialInstitutionAbbreviation = "RBC",
                CsvContent = "", // mocked return from parser
                ShouldIdentifyDuplicates = false,
            };
            
            var response = await sut.Handle(request, CancellationToken.None);

            response.Transactions.Any(t => t.IsLikelyDuplicate).ShouldBeFalse();
        }

        [Fact]
        public void IdentifyDuplicates_ReturnsListWithDuplicatesIdentified()
        {
            var sut = new ParseCsvHandler(
                new XUnitLogger<ParseCsvHandler>(_output), 
                A.Fake<IBankContext>(), 
                new List<ITransactionParser>() { A.Fake<ITransactionParser>() }, 
                A.Fake<IMapper>());
            var account = new Account() { AccountNumber = "12345" };
            var transactions = new List<Transaction>
            {
                new Transaction(account, new DateTime(2020,01,10), 100) { Id = 1 },
                new Transaction(account, new DateTime(2020,01,10), 100) { Id = 2 },
                new Transaction(account, new DateTime(2020,01,11), 120.4m) { Id = 3 },
                new Transaction(account, new DateTime(2020,01,11), 120.4m) { Id = 4 },
                new Transaction(account, new DateTime(2020,01,09), 12) { Id = 5 },
            };
            var transactionModels = new List<TransactionModel>()
            {
                new TransactionModel { AccountNumber = "12345", Amount = 100, Currency = "CAD", Date = new DateTime(2020, 01, 10) },
                //new TransactionModel { AccountNumber = "12345", Amount = 120.4m, Currency = "CAD", Date = new DateTime(2020, 01, 11) },
                //new TransactionModel { AccountNumber = "12345", Amount = 120.4m, Currency = "CAD", Date = new DateTime(2020, 02, 11) },
            };

            var filteredList = sut.IdentifyDuplicates(transactionModels, transactions);

            filteredList.First().IsLikelyDuplicate.ShouldBeTrue();
            filteredList.First().DuplicateIds.Count.ShouldBe(2);
            filteredList.First().DuplicateIds.ShouldBe(new int[] { 1, 2 });
        }

        [Fact]
        public void IdentifyDuplicates_DoesNotMutateInputList()
        {
            var sut = new ParseCsvHandler(
                new XUnitLogger<ParseCsvHandler>(_output),
                A.Fake<IBankContext>(),
                new List<ITransactionParser>() { A.Fake<ITransactionParser>() },
                A.Fake<IMapper>());
            var account = new Account() { AccountNumber = "12345" };
            var transactions = new List<Transaction>
            {
                new Transaction(account, new DateTime(2020,01,10), 100),
                new Transaction(account, new DateTime(2020,01,11), 120.4m),
                new Transaction(account, new DateTime(2020,01,09), 12),
            };
            var transactionModels = new List<TransactionModel>()
            {
                new TransactionModel { AccountNumber = "12345", Amount = 100, Currency = "CAD", Date = new DateTime(2020, 01, 10) },
                new TransactionModel { AccountNumber = "12345", Amount = 120.4m, Currency = "CAD", Date = new DateTime(2020, 01, 11) },
                new TransactionModel { AccountNumber = "12345", Amount = 120.4m, Currency = "CAD", Date = new DateTime(2020, 02, 11) },
            };

            var filteredList = sut.IdentifyDuplicates(transactionModels, transactions);

            transactionModels.Any(t => t.DuplicateIds.Count > 0).ShouldBeFalse();
        }

       
    }
}
