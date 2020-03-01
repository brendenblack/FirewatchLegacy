using Blackbox.Firewatch.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Features
{
    /// <summary>
    /// A secured request that requires certain 
    /// </summary>
    public abstract class AuthorizationRequiredRequest
    {
        /// <summary>
        /// What roles are authorized to perform this request.
        /// </summary>
        public abstract string[] AuthorizedRoles { get; }
        
        /// <summary>
        /// The id of the user who has submitted this request.
        /// </summary>
        public string RequestorId { get; set; }
    }
}
