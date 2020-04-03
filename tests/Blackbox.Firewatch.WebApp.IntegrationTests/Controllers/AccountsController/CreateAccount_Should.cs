using Blackbox.Firewatch.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.WebApp.Controllers.AccountsController;

namespace Blackbox.Firewatch.WebApp.IntegrationTests.Controllers.AccountsController
{
    public class CreateAccount_Should : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateAccount_Should(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnCreated()
        {
            var bodyContent = new CreateAccountModel
            {
                AccountNumber = "abc-1234",
                Bank = "RBC",
                Nickname = "My Account",
            };
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);

            var response = await client.PostAsync("/api/accounts", IntegrationTestHelper.GetRequestContent(bodyContent));

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task ReturnId()
        {
            var bodyContent = new CreateAccountModel
            {
                AccountNumber = "abc-4321",
                Bank = "RBC",
                Nickname = "My Account",
            };
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);

            var response = await client.PostAsync("/api/accounts", IntegrationTestHelper.GetRequestContent(bodyContent));

            var id = await IntegrationTestHelper.GetResponseContent<int>(response);
            using (var db = _factory.CreateContext())
            {
                db.Accounts.Any(a => a.Id == id).ShouldBeTrue();
            }
        }

    }
}
