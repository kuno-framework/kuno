/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Services
{
    /// <summary>
    /// Defines an HTTP endpoint for a service.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EndPointAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndPointAttribute" /> class.
        /// </summary>
        /// <param name="path">The endpoint path.</param>
        public EndPointAttribute(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Gets or sets the HTTP method.  The default is "POST".
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public string Method { get; set; } = "POST";

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        /// <value>The endpoint name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The name.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this endpoint is public.
        /// </summary>
        /// <value><c>true</c> if public; otherwise, <c>false</c>.</value>
        public bool Public { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the endpoint is secure.  Setting to secure will require the caller to be authenticated, otherwise
        /// it will not force the caller to be authenticated.  The user's identity is still available when false.  The default is false.
        /// </summary>
        /// <value><c>true</c> if secure; otherwise, <c>false</c>.</value>
        public bool Secure { get; set; }

        /// <summary>
        /// Gets or sets the endpoint tags.
        /// </summary>
        /// <value>
        /// The endpoint tags.
        /// </value>
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the endpoint timeout in milliseconds.
        /// </summary>
        /// <value>The endpoint timeout in milliseconds.</value>
        public double Timeout { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        /// <value>The version number.</value>
        public int Version { get; set; } = 1;
    }
}