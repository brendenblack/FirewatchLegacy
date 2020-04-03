using Blackbox.Firewatch.Application.Features.Accounts.Queries.GetAccountSummaries;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blackbox.Firewatch.WebApp.IntegrationTests.Controllers.AccountsController
{
    public class GetAccountSummaries_Should : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetAccountSummaries_Should(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnAccountSummaries()
        {
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            using (var db = _factory.CreateContext())
            {
                db.Accounts.AddRange(
                    new Account
                    {
                        OwnerId = TestHarness.StandardUser1.Id,
                        AccountNumber = "1234abc",
                        Institution = FinancialInstitution.RoyalBank,
                    },
                    new Account
                    {
                        OwnerId = TestHarness.StandardUser1.Id,
                        AccountNumber = "5678abc",
                        Institution = FinancialInstitution.RoyalBank,
                    }
                );
                await db.SaveChangesAsync();
            }
            

            var response = await client.GetAsync("/api/accounts");
            var model = await IntegrationTestHelper.GetResponseContent<GetAccountSummariesResponse>(response);

            // because the test uses a shared context we can't test an absolute value
            model.Accounts.Count.ShouldBeGreaterThanOrEqualTo(2); 
            model.Accounts.Any(a => a.AccountNumber == "1234abc").ShouldBeTrue();
            model.Accounts.Any(a => a.AccountNumber == "5678abc").ShouldBeTrue();

        }
    }
}
