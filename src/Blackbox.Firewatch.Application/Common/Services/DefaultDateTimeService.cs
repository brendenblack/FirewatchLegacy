using Blackbox.Firewatch.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Common.Services
{
    public class DefaultDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
