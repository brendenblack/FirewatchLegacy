using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Security;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Infrastructure.Persistence;
using Blackbox.Firewatch.Infrastructure.Persistence.Identity;
using Blackbox.Firewatch.Persistence;
using Blackbox.Firewatch.WebApp.Infrastructure;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.WebApp.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // override the application's configured databases with an in-memory substitute
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDb");
                });

                // Register test services
                //services.AddSingleton<ICurrentUserService, TestCurrentUserService>();
                services.AddScoped<ICurrentUserService, HttpCurrentUserService>();
                //services.AddScoped<IDateTime, TestDateTimeService>();
                //services.AddScoped<IIdentityService, TestIdentityService>();

                _serviceProvider = services.BuildServiceProvider();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    context.Database.EnsureCreated();

                    try
                    {
                        SeedSampleData(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
                    }

                    //var identityService = scopedServices.GetRequiredService<IIdentityService>();
                    //SeedTestUsers(identityService);
                }
            }).UseEnvironment("Test");
        }

        private IServiceProvider _serviceProvider;

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(bool setAsCurrentUser = true)
        {
            return await GetAuthenticatedClientAsync("testaccount@blackbox", "password");
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(TestUserModel user)
        {
            return await GetAuthenticatedClientAsync(user.Username, user.Password);
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(string username, string password, bool setAsCurrentUser = true)
        {
            var client = CreateClient();

            var token = await GetAccessTokenAsync(client, username, password);

            client.SetBearerToken(token);

            if (setAsCurrentUser)
            {
                //    var currentUserService = _serviceProvider.GetRequiredService<ICurrentUserService>();
                //    (currentUserService as TestCurrentUserService).UserId = "abcd";
            }

            return client;
        }

        public ApplicationDbContext CreateContext()
        {
            var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        private async Task<string> GetAccessTokenAsync(HttpClient client, string username, string password)
        {
            var disco = await client.GetDiscoveryDocumentAsync();

            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }
            
            // These string values for ClientId and Scope are established in the Persistence project's ServiceCollection extensions
            var request = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "Blackbox.Firewatch.WebApp.IntegrationTests",
                ClientSecret = "secret",
                Scope = "Blackbox.Firewatch.WebAppAPI openid profile",
                UserName = username,
                Password = password
            };
            var response = await client.RequestPasswordTokenAsync(request);

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            return response.AccessToken;
        }

        public static void SeedSampleData(ApplicationDbContext context)
        {
            // TODO
            context.People.AddRange(
                new Person { Id = TestHarness.StandardUser1.Id },
                new Person { Id = TestHarness.StandardUser2.Id });


            var account = new Account
            {
                AccountNumber = "123-readonly",
                Institution = FinancialInstitution.RoyalBank,
                OwnerId = TestHarness.StandardUser1.Id,
            };
            account.Transactions.Add(new Transaction(account, new DateTime(2019, 01, 10), 100));
            account.Transactions.Add(new Transaction(account, new DateTime(2019, 02, 10), 100));
            account.Transactions.Add(new Transaction(account, new DateTime(2019, 03, 10), 100));
            account.Transactions.Add(new Transaction(account, new DateTime(2019, 04, 10), 100));
            account.Transactions.Add(new Transaction(account, new DateTime(2019, 05, 10), 100));
            context.Accounts.Add(account);


            context.SaveChanges();
        }


        //public async Task SeedTestUsers(IIdentityService identityService)
        //{
        //    var standardUser1Response = await identityService.CreateUserAsync("standarduser1@blackbox", "password");
        //    StandardUser1 = new TestUserModel(standardUser1Response.UserId, "standarduser1@blackbox", "password");

        //    var standardUser2Response = await identityService.CreateUserAsync("standarduser2@blackbox", "password");
        //    StandardUser2 = new TestUserModel(standardUser2Response.UserId, "standarduser2@blackbox", "password");

        //    var adminUserResponse = await identityService.CreateUserAsync("adminuser@blackbox", "password", Roles.Administrator);
        //    AdminUser = new TestUserModel(adminUserResponse.UserId, "adminuser@blackbox", "password");
        //}

        //public TestUserModel StandardUser1 { get; private set; }
        //public TestUserModel StandardUser2 { get; private set; }
        //public TestUserModel AdminUser { get; private set; }

        
    }

    
}
