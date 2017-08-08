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
    /// Request to publish events using the provided JSON text.
    /// </summary>
    public class PublishEventsRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishEventsRequest" /> class.
        /// </summary>
        /// <param name="content">The events to publish serialized as JSON.</param>
        public PublishEventsRequest(string content)
        {
            this.Content = content;
        }

        /// <summary>
        /// Gets the events to publish serialized as JSON.
        /// </summary>
        /// <value>The events to publish serialized as JSON.</value>
        [Json("The content must be JSON.")]
        public string Content { get; }
    }
}