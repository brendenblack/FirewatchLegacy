using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Features
{
    public abstract class PersonScopedAuthorizationRequiredRequest : AuthorizationRequiredRequest
    {
        /// <summary>
        /// The id of the person in question for this request.
        /// </summary>
        public string OwnerId { get; set; }
    }
}
