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
    public class ParseTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ParseTests(ITestOutputHelper output, CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task FirstTest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var query = new ParseCsvModel
            {
                FinancialInstitution = "RBC",
                IsIdentifyDuplicates = false,
                CsvContents = ""
            };
            var content = IntegrationTestHelper.GetRequestContent(query);

            var response = await client.PostAsync($"/api/transactions/parse", content);

            response.IsSuccessStatusCode.ShouldBeTrue($"{response.StatusCode}: {response.ReasonPhrase}");
        }
    }
}
