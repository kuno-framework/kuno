/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// Maintains an inventory of remote services and updates them when necessary.
    /// </summary>
    public class RemoteServiceInventory
    {
        private readonly ConcurrentDictionary<string, RemoteService> _services = new ConcurrentDictionary<string, RemoteService>();

        /// <summary>
        /// Gets the remote services.
        /// </summary>
        /// <value>The remote services.</value>
        public IEnumerable<RemoteService> Services => _services.Values;

        /// <summary>
        /// Gets the remote endpoints.
        /// </summary>
        /// <value>The remote endpoints.</value>
        public IEnumerable<RemoteEndPoint> EndPoints => _services.Values.SelectMany(e => e.EndPoints).AsEnumerable();

        /// <summary>
        /// Adds the specified remote service.
        /// </summary>
        /// <param name="service">The remote service.</param>
        public void Add(RemoteService service)
        {
            _services.AddOrUpdate(service.Path, service, (a, b) => service);
        }
    }
}