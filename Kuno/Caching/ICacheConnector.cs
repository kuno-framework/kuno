/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kuno.Caching
{
    /// <summary>
    /// Defines a contract for a cache connector.  A cache connector is used for messaging between distributed caches.
    /// </summary>
    public interface ICacheConnector
    {
        /// <summary>
        /// Attaches an event handler for when a message is received.
        /// </summary>
        /// <param name="action">The action to perform when a message is received.</param>
        void OnReceived(Action<CacheUpdatedMessage> action);

        /// <summary>
        /// Publishes a message that the items for the specified keys have been modified and should be invalidated.
        /// </summary>
        /// <param name="keys">The keys of the items to invalidate.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        Task PublishChangesAsync(IEnumerable<string> keys);
    }
}