using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kuno.Configuration;
using Kuno.Reflection;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// Contains a registry of all functions, their endpoints and subscriptions.
    /// </summary>
    public class ServiceRegistry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistry"/> class.
        /// </summary>
        /// <param name="application">The current application.</param>
        public ServiceRegistry(ApplicationInformation application)
        {
            this.ApplicationInformation = application;
        }

        /// <summary>
        /// Gets the registered functions.
        /// </summary>
        /// <value>
        /// The registered functions.
        /// </value>
        public List<FunctionInfo> Functions { get; } = new List<FunctionInfo>();

        /// <summary>
        /// Gets the registered endpoints.
        /// </summary>
        /// <value>
        /// The registered endpoints.
        /// </value>
        public EndPointCollection EndPoints { get; } = new EndPointCollection();

        /// <summary>
        /// Gets the registered subscriptions.
        /// </summary>
        /// <value>
        /// The registered subscriptions.
        /// </value>
        public SubscriptionCollection Subscriptions { get; } = new SubscriptionCollection();

        /// <summary>
        /// Gets the application that contain the endpoints.
        /// </summary>
        /// <value>
        /// The application that contain the endpoints.
        /// </value>
        public ApplicationInformation ApplicationInformation { get; private set; }

        /// <summary>
        /// Loads the registry using the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void Load(params Assembly[] assemblies)
        {
            foreach (var service in assemblies.SafelyGetTypes(typeof(IFunction)).Distinct())
            {
                if (!service.GetTypeInfo().IsGenericType && !service.IsDynamic() && !service.GetTypeInfo().IsAbstract)
                {
                    this.Functions.AddRange(FunctionInfo.Create(service));
                }
            }

            foreach (var function in this.Functions)
            {
                this.EndPoints.AddRange(EndPoint.Create(function));
                this.Subscriptions.AddRange(Subscription.Create(function));
            }
        }
    }
}