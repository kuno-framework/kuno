/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// An external service that is available to consume.
    /// </summary>
    public class RemoteService
    {
        /// <summary>
        /// Gets or sets the remote endpoints available on the service.
        /// </summary>
        /// <value>The remote endpoints available on the service.</value>
        public List<RemoteEndPoint> EndPoints { get; set; } = new List<RemoteEndPoint>();

        /// <summary>
        /// Gets or sets the remote service path.
        /// </summary>
        /// <value>The remote service path.</value>
        public string Path { get; set; }
    }
}