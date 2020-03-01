using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
    }
}
