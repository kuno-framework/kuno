/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Used to indicate the path of an external request.
    /// </summary>
    public class RequestAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestAttribute" /> class.
        /// </summary>
        /// <param name="path">The request path.</param>
        public RequestAttribute(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Gets or sets the request method.  This can be different based on the integrated system.  HTTP for example would be GET, POST, etc.
        /// </summary>
        /// <value>The request method.</value>
        public string Method { get; set; }

        /// <summary>
        /// Gets the request path.
        /// </summary>
        /// <value>The request path.</value>
        public string Path { get; }
    }
}