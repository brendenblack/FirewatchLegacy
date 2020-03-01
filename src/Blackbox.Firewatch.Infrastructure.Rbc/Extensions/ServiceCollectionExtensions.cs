using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Infrastructure.Rbc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRoyalBankSupport(this IServiceCollection services)
        {
            services.AddTransient<ITransactionParser, RbcTransactionParser>();
            return services;
        }
    }
}
