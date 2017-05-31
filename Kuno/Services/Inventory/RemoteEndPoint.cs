/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Services.Inventory
{
    /// <summary>
    /// An available endpoint in a remote service.
    /// </summary>
    public class RemoteEndPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteEndPoint" /> class.
        /// </summary>
        /// <param name="path">The relative path the endpoint.</param>
        /// <param name="fullPath">The full path the endpoint.</param>
        /// <param name="method">The endpoint method.</param>
        public RemoteEndPoint(string path, string fullPath, string method = null)
        {
            this.Path = path;
            this.FullPath = fullPath;
            this.Method = method;
        }

        /// <summary>
        /// Gets the full path or address of the endpoint.
        /// </summary>
        /// <value>The full path the endpoint.</value>
        public string FullPath { get; internal set; }

        /// <summary>
        /// Gets or sets the endpoint method.
        /// </summary>
        /// <value>The endpoint method.</value>
        public string Method { get; set; }

        /// <summary>
        /// Gets the relative path the endpoint.  Used to identify the endpoint.
        /// </summary>
        /// <value>The relative path the endpoint.</value>
        public string Path { get; }
    }
}