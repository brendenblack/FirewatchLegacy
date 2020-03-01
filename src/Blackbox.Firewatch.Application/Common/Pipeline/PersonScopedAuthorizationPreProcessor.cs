using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features;
using Blackbox.Firewatch.Application.Security;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Common.Pipeline
{
    /// <summary>
    /// A preprocessor that will ensure that the provided request is authorized by the system.
    /// </summary>
    /// <exception cref="NotAuthorizedException"></exception>
    /// <typeparam name="TRequest"></typeparam>
    public class PersonScopedAuthorizationPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<PersonScopedAuthorizationPreProcessor<TRequest>> _logger;
        private readonly IIdentityService _identityService;

        public PersonScopedAuthorizationPreProcessor(
            ILogger<PersonScopedAuthorizationPreProcessor<TRequest>> logger,
            IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            // A request is deemed okay if one of the following is true:
            // 1. this is not a request that inherits from PersonScopedAuthorizationRequiredRequest
            // 2. the requestor is an admin;
            // 3. the requestor is the owner; or
            // 4. the requestor has been granted permissions by the owner. (not currently supported)

            if (!typeof(PersonScopedAuthorizationRequiredRequest).IsAssignableFrom(request.GetType()))
            {
                return;
            }

            var personScopedRequest = request as PersonScopedAuthorizationRequiredRequest;

            if (personScopedRequest.RequestorId == personScopedRequest.OwnerId)
            {
                return;
            }

            if (IsAdmin(personScopedRequest.RequestorId))
            {
                // this is an action that needs auditable tracing and is logged at info
                _logger.LogInformation(
                    "Executing {} from admin user {} against owner {}",
                    request.GetType().Name,
                    personScopedRequest.RequestorId,
                    personScopedRequest.OwnerId);
                
                return;
            }

            

            if (IsRequestorDelegatedAuthority(personScopedRequest.OwnerId, personScopedRequest.RequestorId))
            {
                _logger.LogInformation(
                        "Executing {} from delegated authority user {} against owner {}",
                        request.GetType().Name,
                        personScopedRequest.RequestorId,
                        personScopedRequest.OwnerId);
                return;
            }

            throw new NotAuthorizedException();
        }

        /// <summary>
        /// Checks to see if the person making the request is the person the request is scoped to, or 
        /// if the requestor has suitable permissions to act.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool CheckPersonScope(TRequest request)
        {
            if (request.GetType().IsAssignableFrom(typeof(PersonScopedAuthorizationRequiredRequest)))
            {
                var personScopedRequest = request as PersonScopedAuthorizationRequiredRequest;
                if (personScopedRequest.OwnerId == personScopedRequest.RequestorId)
                {
                    // If the requestor is the person in question, no problem
                    return true;
                }
                
                // Now the requestor is trying to act on a person that they are not; their roles
                // may allow for this.
                // TODO: check roles, for now just assume false
                
                return false;
            }

            return true;
        }

        public bool IsAdmin(string requestorId)
        {
            var isAdmin = _identityService.IsUserInRole(requestorId, Roles.Administrator);
            isAdmin.Wait();
            return isAdmin.Result;
        }

        public bool IsRequestorDelegatedAuthority(string delegatorId, string requestorId)
        {
            // TODO
            return false;
        }
    }
}
