using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Settings;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Domain.Expenses;
using Blackbox.Firewatch.Infrastructure.Persistence.Identity;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IUserRepository, IBankContext, IExpenseRepository
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDateTime dateTime)
            : base(options, operationalStoreOptions) {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }


        public DbSet<Person> People { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Category> ExpenseCategories { get; set; }

        //public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            //model.Entity<Expense>()
            //    .ToTable("expenses")
            //    .HasDiscriminator<string>(e => e.ExpenseType)
            //    .HasValue<OneTimeExpense>("one time")
            //    .HasValue<OngoingExpense>("ongoing");

            // All entity configurations can be found in the Configurations folder.
            // These configurations will handle the basic mappings of how an entity should be
            // persisted to the database.
            model.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(model);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedById = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedById = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// A convenience helper for wiring up <see cref="Startup"/> and integration tests
        /// </summary>
        public static string TestClientId => "Blackbox.Firewatch.WebApp.IntegrationTests";

        /// <summary>
        /// A convenience helper for wiring up <see cref="Startup"/> and integration tests
        /// </summary>
        public static string TestClientScope => "Blackbox.Firewatch.WebAppAPI";

    }
}
