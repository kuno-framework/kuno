/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Services.EndPoints
{
    /// <summary>
    /// Request to get responses that have been executed in an application context.
    /// </summary>
    public class GetResponsesRequest
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