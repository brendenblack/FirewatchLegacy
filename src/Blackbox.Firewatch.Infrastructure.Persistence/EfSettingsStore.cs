using Blackbox.Firewatch.Application.Settings;
using Blackbox.Firewatch.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Infrastructure.Persistence
{
    public class EfSettingsStore : ISettingsStore
    {
        private readonly ILogger<EfSettingsStore> logger;

        public EfSettingsStore(
            ILogger<EfSettingsStore> logger, 
            ApplicationDbContext context)
        {
            this.logger = logger;
        }

        public Task<UserSettings> GetSettingsForUser(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
