using Blackbox.Firewatch.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.WebApp.IntegrationTests
{
    public class TestCurrentUserService : ICurrentUserService
    {
        public string UserId => "00000000-0000-0000-0000-000000000000";
    }
}
