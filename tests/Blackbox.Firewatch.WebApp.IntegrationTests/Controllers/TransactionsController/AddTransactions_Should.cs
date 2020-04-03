using Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blackbox.Firewatch.WebApp.IntegrationTests.Controllers.TransactionsController
{
    public class AddTransactions_Should : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public AddTransactions_Should(ITestOutputHelper output, CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenAuthenticated_PersistTransactions()
        {
            var accountNumber = Guid.NewGuid().ToString();
            var client = await _factory.GetAuthenticatedClientAsync(TestHarness.StandardUser1);
            using (var db = _factory.CreateContext())
            {
                var account = new Account
                {
                    AccountNumber = accountNumber,
                    Institution = FinancialInstitution.RoyalBank,
                    OwnerId = TestHarness.StandardUser1.Id,
                };
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
            }
            var bodyContent = new List<AddTransactionModel>
            {
                new AddTransactionModel
                {
                    AccountNumber = accountNumber,
                    Amount = 100,
                    Currency = "CAD",
                    Date = DateTime.Now,
                }
            };

            var response = await client.PostAsync("/api/transactions", IntegrationTestHelper.GetRequestContent(bodyContent));

            response.IsSuccessStatusCode.ShouldBeTrue($"{(int)response.StatusCode}: {response.ReasonPhrase}");
            var model = await IntegrationTestHelper.GetResponseContent<AddTransactionsResponse>(response);
            model.CreatedIds.Count.ShouldBe(1);
            using (var db = _factory.CreateContext())
            {
                foreach (var id in model.CreatedIds)
                {
                    db.Transactions.Any(tx => tx.Id == id).ShouldBeTrue();
                }
            }
        }
    }
}
