using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.WebApp.IntegrationTests
{
    public class TestIdentityService : IIdentityService
    {
        public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(string userId)
        {
            return Task.FromResult("testuser@blackbox");
        }

        public Task<bool> IsUserInRole(string userId, string role)
        {
            throw new NotImplementedException();
        }
    }
}
