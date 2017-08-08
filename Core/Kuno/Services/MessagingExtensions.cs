/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using Autofac;
using Kuno.Services.Logging;
using Kuno.Services.Messaging;

namespace Kuno.Services
{
    /// <summary>
    /// Contains methods to configure a <see cref="KunoStack" />.
    /// </summary>
    public static class MessagingExtensions
    {
        /// <summary>
        /// Gets the event entries that fall within the specified time frame.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Returns the event entries that fall within the specified time frame.</returns>
        public static IEnumerable<EventEntry> GetEvents(this KunoStack instance, DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            return instance.Container.Resolve<IEventStore>().GetEvents(start, end).Result;
        }

        /// <summary>
        /// Gets the request entries that fall within the specified time frame.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Returns the request entries that fall within the specified time frame.</returns>
        public static IEnumerable<RequestEntry> GetRequests(this KunoStack instance, DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            return instance.Container.Resolve<IRequestLog>().GetEntries(start, end).Result;
        }

        /// <summary>
        /// Gets the response entries that fall within the specified time frame.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Returns the response entries that fall within the specified time frame.</returns>
        public static IEnumerable<ResponseEntry> GetResponses(this KunoStack instance, DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            return instance.Container.Resolve<IResponseLog>().GetEntries(start, end).Result;
        }
    }
}