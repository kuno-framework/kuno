/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno.Services.Logging
{
    /// <summary>
    /// Stores and retrieves events.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Appends the specified event instance to the store.
        /// </summary>
        /// <param name="instance">The event instance to add.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        Task Append(EventMessage instance);

        /// <summary>
        /// Gets events that fall within the specified time frame.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>Returns events that fall within the specified time frame.</returns>
        Task<IEnumerable<EventEntry>> GetEvents(DateTimeOffset? start = null, DateTimeOffset? end = null);
    }
}