using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features;
using Blackbox.Firewatch.Application.Security;
using Blackbox.Firewatch.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IdentityService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }
        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string role = Roles.User)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));

            await InitializePersonForUser(user);

            return (result.ToApplicationResult(), user.Id);

        }

        /// <summary>
        /// Ensures that a <see cref="Person"/> record has been created as well as setting up
        /// basic account features like <see cref="Expense"/> categories.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> InitializePersonForUser(ApplicationUser user)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Id == user.Id);
            if (person == null)
            {
                person = new Person() { Id = user.Id };
                _context.Add(person);
                await _context.SaveChangesAsync();
            }

            // TODO: create expense categories for person

            return Result.Ok();
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Ok();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<bool> IsUserInRole(string userId, string role)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
