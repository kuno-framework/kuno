/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.AzureServiceBus.Settings
{
    /// <summary>
    /// Settings for the topic event publisher.
    /// </summary>
    public class TopicEventPublisherSettings
    {
        /// <summary>
        /// Gets or sets the name of the topic.
        /// </summary>
        /// <value>
        /// The name of the topic.
        /// </value>
        public string TopicName { get; set; }
    }
}