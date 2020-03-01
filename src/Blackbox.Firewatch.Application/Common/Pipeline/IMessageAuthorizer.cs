using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Common.Pipeline
{
    public interface IMessageAuthorizer
    {
        /// <summary>
        /// Evaluate whether or not the requesting user has the appropriate role to perform
        /// the requested action.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        bool Evaluate<TRequest>(TRequest request) where TRequest : class;
    }
}
