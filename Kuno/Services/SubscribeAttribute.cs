/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Kuno.Validation;

namespace Kuno.Services
{
    /// <summary>
    /// Indicates the handler should subscribe to a specific channel.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SubscribeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeAttribute" /> class.
        /// </summary>
        /// <param name="channel">The channel that the event handler should subscribe to.</param>
        public SubscribeAttribute(string channel)
        {
            Argument.NotNullOrWhiteSpace(channel, nameof(channel));

            this.Channel = channel;
        }

        /// <summary>
        /// Gets the channel that the event handler should subscribe to.
        /// </summary>
        /// <value>The channel that the event handler should subscribe to.</value>
        public string Channel { get; }
    }
}