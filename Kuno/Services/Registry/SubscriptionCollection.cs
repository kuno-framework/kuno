/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Linq;
using Kuno.Services.Messaging;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// A list of subscriptions with methods to find internal functions.
    /// </summary>
    public class SubscriptionCollection : List<Subscription>
    {
        /// <summary>
        /// Finds the registered endpoints based on the specified event message.
        /// </summary>
        /// <param name="instance">The event message.</param>
        /// <returns>Returns the registered endpoints based on the specified event message.</returns>
        public IEnumerable<Subscription> Find(EventMessage instance)
        {
            foreach (var subscription in this.Where(e => e.Channel == instance.Name))
            {
                yield return subscription;
            }
        }

        /// <summary>
        /// Finds the registered endpoints based on the specified channel.
        /// </summary>
        /// <param name="channel">The event channel.</param>
        /// <returns>Returns the registered endpoints based on the specified event channel.</returns>
        public IEnumerable<Subscription> Find(string channel)
        {
            foreach (var subscription in this.Where(e => e.Channel == channel))
            {
                yield return subscription;
            }
        }
    }
}