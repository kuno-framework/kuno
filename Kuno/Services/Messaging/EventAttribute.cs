/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Defines information about an EventName.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class EventAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the event name.
        /// </summary>
        /// <value>The event name.</value>
        public string Name { get; set; }
    }
}