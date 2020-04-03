using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Infrastructure.Persistence;
using Blackbox.Firewatch.Infrastructure.Persistence.Identity;
using Blackbox.Firewatch.Persistence;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //.AddSignInManager();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            if (environment.IsEnvironment("Test"))
            {
                // Set the stage for integration testing by creating a client and a test user.
                // http://docs.identityserver.io/en/3.1.0/quickstarts/1_client_credentials.html
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                    {
                        options.Clients.Add(new Client
                        {
                            ClientId = "Blackbox.Firewatch.WebApp.IntegrationTests",
                            AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            AllowedScopes = { "Blackbox.Firewatch.WebAppAPI", "openid", "profile" }
                        });
                    }).AddTestUsers(new List<TestUser>
                    {
                        new TestUser
                        {
                            SubjectId = TestHarness.StandardUser1.Id,
                            Username = TestHarness.StandardUser1.Username,
                            Password = TestHarness.StandardUser1.Password,
                        },
                        new TestUser
                        {
                            SubjectId = TestHarness.StandardUser2.Id,
                            Username = TestHarness.StandardUser2.Username,
                            Password = TestHarness.StandardUser2.Password,
                        }
                    });
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            }

            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }

        
    }
}

