using Blackbox.Firewatch.Application.Features;
using Blackbox.Firewatch.Application.Security;
using Blackbox.Firewatch.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string role = Roles.User);

        Task<Result> DeleteUserAsync(string userId);

        Task<bool> IsUserInRole(string userId, string role);

        //Task<Person> RegisterUser();
    }
}
