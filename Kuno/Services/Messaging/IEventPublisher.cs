/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Threading.Tasks;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Publishes events to an external subscriber.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        /// <param name="events">The events to publish.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task Publish(params EventMessage[] events);
    }
}