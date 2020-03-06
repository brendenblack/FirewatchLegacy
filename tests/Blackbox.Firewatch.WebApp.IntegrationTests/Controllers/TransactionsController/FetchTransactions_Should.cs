using Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchTransactions;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Persistence;
using Shouldly;
using System;
using System.Threading.Tasks;
using System.Web;
using Xunit;
using Xunit.Abstractions;

namespace Blackbox.Firewatch.WebApp.IntegrationTests.Controllers.TransactionsController
{
    public class FetchTransactions_Should : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public FetchTransactions_Should(ITestOutputHelper output, CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenAuthenticated_ReturnTransactions()
        {
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            using (var db = _factory.CreateContext())
            {
                var account = new Account
                {
                    AccountNumber = "12345",
                    Institution = FinancialInstitution.RoyalBank,
                    OwnerId = TestHarness.StandardUser1.Id,
                };
                account.Transactions.Add(new Transaction(account, DateTime.Now, 100));
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
            }
            
            var response = await client.GetAsync($"/api/transactions");

            response.IsSuccessStatusCode.ShouldBeTrue($"{(int)response.StatusCode}: {response.ReasonPhrase}");
            var model = await IntegrationTestHelper.GetResponseContent<FetchTransactionsResponse>(response);
            model.Transactions.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ReturnUnauthorized_WhenNotAuthenticated()
        {
            var client = _factory.GetAnonymousClient();
            using (var db = _factory.CreateContext())
            {
                var account = new Account
                {
                    AccountNumber = "12345",
                    Institution = FinancialInstitution.RoyalBank,
                    OwnerId = TestHarness.StandardUser1.Id,
                };
                account.Transactions.Add(new Transaction(account, DateTime.Now, 100));
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
            }

            var response = await client.GetAsync($"/api/transactions");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task GivenAuthenticated_ReturnTransactionsLaterThanFromDate()
        {
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            using (var db = _factory.CreateContext())
            {
                var account = new Account
                {
                    AccountNumber = "12345",
                    Institution = FinancialInstitution.RoyalBank,
                    OwnerId = TestHarness.StandardUser1.Id,
                };
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 01, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 02, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 03, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 04, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 05, 10), 100));
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
            }
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["from"] = "20190201";

            var response = await client.GetAsync($"/api/transactions?{query.ToString()}");

            response.IsSuccessStatusCode.ShouldBeTrue($"{(int)response.StatusCode}: {response.ReasonPhrase}");
            var model = await IntegrationTestHelper.GetResponseContent<FetchTransactionsResponse>(response);
            model.Transactions.Count.ShouldBe(4);
        }

        [Fact]
        public async Task GivenAuthenticated_ReturnTransactionsEarlierThanToDate()
        {
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            using (var db = _factory.CreateContext())
            {
                var account = new Account
                {
                    AccountNumber = "12345",
                    Institution = FinancialInstitution.RoyalBank,
                    OwnerId = TestHarness.StandardUser1.Id,
                };
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 01, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 02, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 03, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 04, 10), 100));
                account.Transactions.Add(new Transaction(account, new DateTime(2019, 05, 10), 100));
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
            }
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["from"] = "20190201";

            var response = await client.GetAsync($"/api/transactions?{query.ToString()}");

            response.IsSuccessStatusCode.ShouldBeTrue($"{(int)response.StatusCode}: {response.ReasonPhrase}");
            var model = await IntegrationTestHelper.GetResponseContent<FetchTransactionsResponse>(response);
            model.Transactions.Count.ShouldBe(4);
        }
    }
}
