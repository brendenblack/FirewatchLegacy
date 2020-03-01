using Blackbox.Firewatch.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Settings
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This interface cannot be moved to the Abstractions package because it depends on
    /// <see cref="UserSettings"/>, which is not a domain (enterprise) concern, and thus is
    /// provided by the application layer. 
    /// </remarks>
    public interface ISettingsStore
    {
        Task<UserSettings> GetSettingsForUser(Person person);
    }
}
