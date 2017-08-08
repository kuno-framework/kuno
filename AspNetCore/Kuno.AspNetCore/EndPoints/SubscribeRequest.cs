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
    /// Requests an event subscription to the specified URL.
    /// </summary>
    public class SubscribeRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeRequest" /> class.
        /// </summary>
        /// <param name="url">The URL to publish to.</param>
        public SubscribeRequest(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// Gets the URL to publish to.
        /// </summary>
        /// <value>The URL to publish to.</value>
        [Url("The URL to publish to must be a well-formed URL.")]
        public string Url { get; }
    }
}