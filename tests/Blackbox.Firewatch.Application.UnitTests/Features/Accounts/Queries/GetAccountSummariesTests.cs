using Blackbox.Firewatch.Application.Features.Accounts.Queries.GetAccountSummaries;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.Application.Features.Accounts.Queries.GetAccountSummaries.GetAccountSummariesQuery;

namespace Blackbox.Firewatch.Application.UnitTests.Features.Accounts.Queries
{
    [Collection("QueryTests")]
    public class GetAccountSummariesTests 
    {
        private readonly ITestOutputHelper _output;
        private readonly QueryTestFixture _fixture;

        public GetAccountSummariesTests(ITestOutputHelper output, QueryTestFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task ReturnAccountSummaries()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            _fixture.Context.People.Add(person);
            var account1 = new Account 
            { 
                OwnerId = person.Id,
                AccountNumber = "abc-123",
                Institution = FinancialInstitution.FromAbbreviation("RBC"),
            };
            var account2 = new Account
            {
                OwnerId = person.Id,
                AccountNumber = "abc-456",
                Institution = FinancialInstitution.FromAbbreviation("RBC"),
            };
            account2.AddTransaction(DateTime.Now, new string[0], 100, "CAD");
            account2.AddTransaction(DateTime.Now, new string[0], 200, "CAD");
            _fixture.Context.Accounts.AddRange(account1, account2);
            await _fixture.Context.SaveChangesAsync();
            var sut = new GetAccountSummariesHandler(new XUnitLogger<GetAccountSummariesHandler>(_output), _fixture.Context);
            var query = new GetAccountSummariesQuery { PersonId = person.Id };

            var result = await sut.Handle(query, CancellationToken.None);

            result.Accounts.Count.ShouldBe(2);
        }

        [Fact]
        public async Task ReturnExpectedAccountSummaries()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            _fixture.Context.People.Add(person);
            var account1 = new Account
            {
                OwnerId = person.Id,
                AccountNumber = "abc-123",
                Institution = FinancialInstitution.FromAbbreviation("RBC"),
            };
            var account2 = new Account
            {
                OwnerId = person.Id,
                AccountNumber = "abc-456",
                Institution = FinancialInstitution.FromAbbreviation("RBC"),
            };
            account2.AddTransaction(new DateTime(2019,01,01), new string[0], 100, "CAD");
            account2.AddTransaction(new DateTime(2019, 12, 31), new string[0], 200, "CAD");
            _fixture.Context.Accounts.AddRange(account1, account2);
            await _fixture.Context.SaveChangesAsync();
            var sut = new GetAccountSummariesHandler(new XUnitLogger<GetAccountSummariesHandler>(_output), _fixture.Context);
            var query = new GetAccountSummariesQuery { PersonId = person.Id };

            var result = await sut.Handle(query, CancellationToken.None);

            var account = result.Accounts.First(a => a.AccountNumber == "abc-456");
            account.Transactions.ShouldBe(2);
            account.EarliestTransaction.ShouldBe(new DateTime(2019, 01, 01));
            account.LatestTransaction.ShouldBe(new DateTime(2019, 12, 31));
        }
    }
}
