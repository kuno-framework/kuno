/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kuno.Configuration;
using Kuno.Reflection;
using Kuno.Services.Messaging;

namespace Kuno.Services.Inventory
{
    /// <summary>
    /// A service inventory containing available services.
    /// </summary>
    public class ServiceInventory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInventory"/> class.
        /// </summary>
        /// <param name="application">The application that contain the endpoints.</param>
        public ServiceInventory(ApplicationInformation application)
        {
            this.ApplicationInformation = application;
        }

        /// <summary>
        /// Gets the application that contain the endpoints.
        /// </summary>
        /// <value>
        /// The application that contain the endpoints.
        /// </value>
        public ApplicationInformation ApplicationInformation { get; }

        /// <summary>
        /// Gets or sets the inventoried end points.
        /// </summary>
        /// <value>
        /// The inventoried end points.
        /// </value>
        public List<EndPointMetaData> EndPoints { get; set; } = new List<EndPointMetaData>();

        /// <summary>
        /// Finds the endpoint for the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns the endpoint for the specified message.</returns>
        public IEnumerable<EndPointMetaData> Find(object message)
        {
            if (message == null)
            {
                return Enumerable.Empty<EndPointMetaData>();
            }
            return this.EndPoints.Where(e => e.RequestType == message.GetType());
        }

        /// <summary>
        /// Finds the endpoint for the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Returns the endpoint for the specified path.</returns>
        public EndPointMetaData Find(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            path = path.Trim('/');

            return this.EndPoints.FirstOrDefault(e => e.Path == path);
        }

        /// <summary>
        /// Finds the endpoint for the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns the endpoint for the specified message.</returns>
        public IEnumerable<EndPointMetaData> Find(EventMessage message)
        {
            return this.EndPoints.Where(e => e.RequestType == message.MessageType || e.EndPointType.GetAllAttributes<SubscribeAttribute>().Any());
        }

        /// <summary>
        /// Finds the endpoint that can handle the specified path.  If there is no path, then the message will be used.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns the endpoint.</returns>
        public EndPointMetaData Find(string path, object message)
        {
            var target = this.Find(path);
            if (message != null)
            {
                if (target == null)
                {
                    target = this.Find(message).FirstOrDefault();
                }
                if (target == null)
                {
                    var attribute = message.GetType().GetAllAttributes<RequestAttribute>().FirstOrDefault();
                    if (attribute != null)
                    {
                        target = this.Find(attribute.Path);
                    }
                }
            }
            return target;
        }

        /// <summary>
        /// Loads all local services using the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to use to scan.</param>
        public void Load(params Assembly[] assemblies)
        {
            foreach (var service in assemblies.SafelyGetTypes(typeof(IService)).Distinct())
            {
                if (!service.GetTypeInfo().IsGenericType && !service.IsDynamic() && !service.GetTypeInfo().IsAbstract)
                {
                    this.EndPoints.AddRange(EndPointMetaData.Create(service));
                }
            }
            foreach (var group in this.EndPoints.GroupBy(e => e.Path))
            {
                var current = group.Max(e => e.Version);
                foreach (var previous in group.Where(e => e.Version != current))
                {
                    previous.Path = $"v{previous.Version}/{previous.Path}";
                    previous.IsVersioned = true;
                }
            }
        }
    }
}