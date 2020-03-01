using Blackbox.Firewatch.Application.Features;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Ok()
                : Result.Fail(string.Join("\n", result.Errors.Select(e => e.Description)));
        }
    }
}
