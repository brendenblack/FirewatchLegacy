using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Common.Pipeline
{
    public class PermissiveMessageAuthorizer : IMessageAuthorizer
    {
        private readonly ILogger<PermissiveMessageAuthorizer> logger;

        public PermissiveMessageAuthorizer(ILogger<PermissiveMessageAuthorizer> logger)
        {
            this.logger = logger;
        }

        public bool Evaluate<TRequest>(TRequest request) where TRequest : class
        {
#if !DEBUG
            logger.LogWarning("This application is in permissive mode. No security rules are being enforced. This should only be used in a development environment!");
#endif
            return true;
        }
    }
}
