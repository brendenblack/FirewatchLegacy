using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Infrastructure.Persistence;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Persistence
{
    public class Repository
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public Repository(DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDateTime dateTime)
        {
            _options = options;
            _operationalStoreOptions = operationalStoreOptions;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public ApplicationDbContext CreateContext()
        {
            return new ApplicationDbContext(_options, _operationalStoreOptions, _currentUserService, _dateTime);
        }
    }
}
