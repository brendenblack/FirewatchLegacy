using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchTransactions;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using FakeItEasy;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchTransactions.FetchTransactionsQuery;

namespace Blackbox.Firewatch.Application.UnitTests.Features.Transactions.Queries
{
    [Collection("QueryTests")]
    public class FetchTransactionsTests
    {
        private readonly ITestOutputHelper _output;
        private readonly QueryTestFixture _fixture;

        public FetchTransactionsTests(ITestOutputHelper output, QueryTestFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_ReturnsList_ForOwnerRequestor()
        {
            var logger = new XUnitLogger<FetchTransactionsHandler>(_output);
            var db = ApplicationDbContextFactory.Create();
            var person = new Person { Id = Guid.NewGuid().ToString() };
            db.People.Add(person);
            await db.SaveChangesAsync();
            var account = new Account
            {
                Owner = person,
                OwnerId = person.Id,
                AccountNumber = "12345",
                Institution = FinancialInstitution.RoyalBank
            };
            db.Accounts.AddRange(account);
            await db.SaveChangesAsync();
            db.Transactions.AddRange(new List<Transaction>
            {
                new Transaction(account, DateTime.Now, 100)
            });
            await db.SaveChangesAsync();
            var parsers = new List<ITransactionParser>();
            var sut = new FetchTransactionsHandler(logger, db, _fixture.Mapper);
            var request = new FetchTransactionsQuery
            {
                OwnerId = person.Id,
                RequestorId = person.Id,
            };

            var response = await sut.Handle(request, CancellationToken.None);

            response.Transactions.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoTransactions()
        {
            var logger = new XUnitLogger<FetchTransactionsHandler>(_output);
            var db = ApplicationDbContextFactory.Create();
            var person = new Person { Id = Guid.NewGuid().ToString() };
            db.People.Add(person);
            await db.SaveChangesAsync();
            var account = new Account
            {
                Owner = person,
                OwnerId = person.Id,
                AccountNumber = "12345",
                Institution = FinancialInstitution.RoyalBank
            };
            db.Accounts.AddRange(account);
            await db.SaveChangesAsync();
            var parsers = new List<ITransactionParser>();
            var sut = new FetchTransactionsHandler(logger, db, _fixture.Mapper);
            var request = new FetchTransactionsQuery
            {
                OwnerId = person.Id,
                RequestorId = person.Id,
            };

            var response = await sut.Handle(request, CancellationToken.None);

            response.Transactions.ShouldBeEmpty();
        }
    }
}
