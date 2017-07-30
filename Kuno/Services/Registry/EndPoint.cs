/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kuno.Reflection;
using Kuno.Text;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// Contains information about an endpoint.
    /// </summary>
    public class EndPoint
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is an older version.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is an older version; otherwise, <c>false</c>.
        /// </value>
        public bool IsVersioned { get; set; }

        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public FunctionInfo Function { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        /// <value>
        /// The endpoint name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the relative path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        public string VersionedPath => $"v{this.Version}/{this.Path}";

        /// <summary>
        /// Gets or sets a value indicating whether the endpoint is secure.
        /// </summary>
        /// <value>Indicates whether the endpoint is secure.</value>
        public bool Secure { get; set; }

        /// <summary>
        /// Gets or sets the endpoint tags.
        /// </summary>
        /// <value>
        /// The endpoint tags.
        /// </value>
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the endpoint timeout.
        /// </summary>
        /// <value>The endpoint timeout.</value>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the endpoint version number.
        /// </summary>
        /// <value>The endpoint version number.</value>
        public int Version { get; set; }

        /// <summary>
        /// Gets the endpoints for the specified function.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <returns>
        /// Returns the endpoints for the specified function.
        /// </returns>
        public static IEnumerable<EndPoint> Create(FunctionInfo function)
        {
            if (function.FunctionType != null)
            {
                var version = function.FunctionType.GetVersion();
                var timeout = function.FunctionType.GetTimeout();

                yield return new EndPoint
                {
                    Name = function.FunctionType.Name.ToTitle(),
                    HttpMethod = "POST",
                    Version = version,
                    Timeout = timeout,
                    Function = function
                };

                var attributes = function.FunctionType.GetAllAttributes<EndPointAttribute>();
                foreach (var attribute in attributes)
                {
                    yield return new EndPoint
                    {
                        Name = attribute.Name ?? function.FunctionType.Name.ToTitle(),
                        Path = GetPath(attribute),
                        HttpMethod = attribute.Method ?? "POST",
                        Tags = attribute.Tags,
                        Version = version,
                        Timeout = timeout,
                        Secure = attribute.Secure,
                        Function = function
                    };
                }
            }
        }

        private static string GetPath(EndPointAttribute attribute)
        {
            return attribute.Path;
        }
    }
}