using Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchTransactions;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Persistence;
using Shouldly;
using System;
using System.Linq;
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
            
            var response = await client.GetAsync($"/api/transactions");

            response.IsSuccessStatusCode.ShouldBeTrue($"{(int)response.StatusCode}: {response.ReasonPhrase}");
            var model = await IntegrationTestHelper.GetResponseContent<FetchTransactionsResponse>(response);
            model.Transactions.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReturnUnauthorized_WhenNotAuthenticated()
        {
            var client = _factory.GetAnonymousClient();

            var response = await client.GetAsync($"/api/transactions");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task GivenAuthenticated_ReturnTransactionsLaterThanFromDate()
        {
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["from"] = "20190201";

            var response = await client.GetAsync($"/api/transactions?{query.ToString()}");

            response.IsSuccessStatusCode.ShouldBeTrue($"{(int)response.StatusCode}: {response.ReasonPhrase}");
            var model = await IntegrationTestHelper.GetResponseContent<FetchTransactionsResponse>(response);
            model.Transactions.Any(tx => tx.Date.Date < new DateTime(2019, 02, 01).Date).ShouldBeFalse();
        }

        [Fact]
        public async Task GivenAuthenticated_ReturnTransactionsEarlierThanToDate()
        {
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["to"] = "20190201";

            var response = await client.GetAsync($"/api/transactions?{query.ToString()}");

            response.IsSuccessStatusCode.ShouldBeTrue($"{(int)response.StatusCode}: {response.ReasonPhrase}");
            var model = await IntegrationTestHelper.GetResponseContent<FetchTransactionsResponse>(response);
            model.Transactions.Any(tx => tx.Date.Date > new DateTime(2019, 02, 01).Date).ShouldBeFalse();
        }
    }
}
