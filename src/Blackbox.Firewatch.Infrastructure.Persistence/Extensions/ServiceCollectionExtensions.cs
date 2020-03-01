using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Infrastructure.Persistence;
using Blackbox.Firewatch.Infrastructure.Persistence.Identity;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        // TODO: I don't like receiving IConfiguration directly here, this should receive an action.
        public static IServiceCollection AddPersistenceAndIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            // Allow user's to specify which database they want to use, defaulting to SqlServer.
            // For integration testing purposes these definitions will be removed and replaced with an in memory
            // store, so no consideration needs to be given here.
            var database = configuration.GetValue<string>("database");
            switch (database)
            {
                case "postgres":
                case "postgresql":
                    services.AddDbContext<ApplicationDbContext>(opts =>
                        opts.UseNpgsql(
                            configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                    break;
                case "sqlserver":
                default:
                    services.AddDbContext<ApplicationDbContext>(opts =>
                         opts.UseSqlServer(
                             configuration.GetConnectionString("DefaultConnection"),
                             b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))); ;
                    break;
            }
            
            // Register ApplicationDbContext as the provider for each interface that it implements.
            foreach (var i in typeof(ApplicationDbContext).GetInterfaces())
            {
                services.AddScoped(i, typeof(ApplicationDbContext));
            }

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            if (environment.IsEnvironment("Test"))
            {
                // Set the stage for integration testing by creating a client and a test user.
                // http://docs.identityserver.io/en/3.1.0/quickstarts/1_client_credentials.html
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                    {
                        options.Clients.Add(new Client
                        {
                            ClientId = ApplicationDbContext.TestClientId,
                            AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            AllowedScopes = { ApplicationDbContext.TestClientScope, "openid", "profile" }
                        });
                    }).AddTestUsers(new List<TestUser>
                    {
                        new TestUser
                        {
                            SubjectId = "f26da293-02fb-4c90-be75-e4aa51e0bb17",
                            Username = "testaccount@blackbox",
                            Password = "Firewatch"
                        }
                    });
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

                services.AddTransient<IIdentityService, IdentityService>();
            }

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }

        
    }
}

