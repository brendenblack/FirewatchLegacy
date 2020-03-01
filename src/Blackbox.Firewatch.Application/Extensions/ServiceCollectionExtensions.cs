using AutoMapper;
using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Common.Services;
using Blackbox.Firewatch.Application.Infrastructure;
using Blackbox.Firewatch.Application.Common.Pipeline;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the basic services required to run Firewatch.
        /// <para>
        /// The following interfaces must be provided by the container:
        /// <list type="bullet">
        ///     <item><description><see cref="IMessageAuthorizer"/></description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFirewatch(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register FluentValidation validators automatically. 
            // https://github.com/JeremySkinner/FluentValidation/blob/d97d071d023ec20896cbd548cfb7a10ee30554d7/src/FluentValidation/AssemblyScanner.cs#L91-L97
            var openGenericType = typeof(IValidator<>);
            var validators = from type in Assembly.GetAssembly(typeof(ServiceCollectionExtensions)).ExportedTypes
                             where !type.IsAbstract && !type.IsGenericTypeDefinition
                             let interfaces = type.GetInterfaces()
                             let genericInterfaces = interfaces.Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
                             let matchingInterface = genericInterfaces.FirstOrDefault()
                             where matchingInterface != null
                             select new AssemblyScanResult(matchingInterface, type);

            foreach (var v in validators)
            {
                services.AddTransient(v.InterfaceType, v.ValidatorType);
            }


#if DEBUG
            // This service is registered here to allow for the application to run in debug environment, and must not be used live.
            services.AddTransient<IMessageAuthorizer, PermissiveMessageAuthorizer>();
#endif

            // Register MediatR and all handlers found in the assembly, along with pipeline behaviors. 
            // Behaviours are executed in the order they are registered so we start with authorization to 
            // limit possible exploits.
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(PersonScopedAuthorizationPreProcessor<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IDateTime, DefaultDateTimeService>();

            return services;
        }

        /// <summary>
        /// Result of performing a scan.
        /// </summary>
        private class AssemblyScanResult
        {
            /// <summary>
            /// Creates an instance of an AssemblyScanResult.
            /// </summary>
            public AssemblyScanResult(Type interfaceType, Type validatorType)
            {
                InterfaceType = interfaceType;
                ValidatorType = validatorType;
            }

            /// <summary>
            /// Validator interface type, eg IValidator&lt;Foo&gt;
            /// </summary>
            public Type InterfaceType { get; private set; }

            /// <summary>
            /// Concrete type that implements the InterfaceType, eg FooValidator.
            /// </summary>
            public Type ValidatorType { get; private set; }
        }
    }
}
