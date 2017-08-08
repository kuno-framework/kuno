/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.Validation;

namespace Kuno.AspNetCore.EndPoints
{
    /// <summary>
    /// Requests events from a remote feed and then publishes them to any listeners locally.
    /// </summary>
    public class ConsumeEventFeedRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumeEventFeedRequest" /> class.
        /// </summary>
        /// <param name="url">The URL to the event feed.</param>
        public ConsumeEventFeedRequest(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// Gets the URL to the event feed.
        /// </summary>
        /// <value>The URL to the event feed.</value>
        [Url("The event feed URL must be a well-formed URL.")]
        public string Url { get; }
    }
}