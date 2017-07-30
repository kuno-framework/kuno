/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using Kuno.Reflection;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// Represents a function subscription to a message channel.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public FunctionInfo Function { get; set; }

        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public string Channel { get; set; }

        /// <summary>
        /// Creates subscriptions based on the specified function.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <returns>Returns created subscriptions based on the specified function.</returns>
        public static IEnumerable<Subscription> Create(FunctionInfo function)
        {
            var attributes = function.FunctionType.GetAllAttributes<SubscribeAttribute>();
            foreach (var attribute in attributes)
            {
                yield return new Subscription
                {
                    Function = function,
                    Channel = attribute.Channel
                };
            }
        }
    }
}