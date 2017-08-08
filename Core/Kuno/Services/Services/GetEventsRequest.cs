/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Services.Services
{
    /// <summary>
    /// Request to get the events that have been raised within the application context.
    /// </summary>
    public class GetEventsRequest
    {
        /// <summary>
        /// Gets the end of the requested range.
        /// </summary>
        /// <value>The end of the requested range.</value>
        public DateTimeOffset? End { get; set; }

        /// <summary>
        /// Gets the start of the requested range.
        /// </summary>
        /// <value>The start of the requested range.</value>
        public DateTimeOffset? Start { get; set; }
    }
}