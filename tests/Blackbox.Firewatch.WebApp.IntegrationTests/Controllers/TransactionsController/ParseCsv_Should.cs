using Blackbox.Firewatch.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.WebApp.Controllers.TransactionsController;

namespace Blackbox.Firewatch.WebApp.IntegrationTests.Controllers.TransactionsController
{
    public class ParseCsv_Should : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ParseCsv_Should(ITestOutputHelper output, CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnSuccessCodeOnSuccessfulRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            var query = new ParseCsvModel
            {
                FinancialInstitution = "RBC",
                IsIdentifyDuplicates = false,
                CsvContents = ""
            };
            var content = IntegrationTestHelper.GetRequestContent(query);

            var response = await client.PostAsync($"/api/users/{TestHarness.StandardUser1.Id}/transactions/parse", content);

            response.IsSuccessStatusCode.ShouldBeTrue($"{response.StatusCode}: {response.ReasonPhrase}");
        }
    }
}
