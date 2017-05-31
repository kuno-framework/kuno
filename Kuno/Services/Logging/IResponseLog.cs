/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kuno.Services.Logging
{
    /// <summary>
    /// A log that tracks responses.
    /// </summary>
    public interface IResponseLog
    {
        /// <summary>
        /// Appends the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task Append(ResponseEntry entry);

        /// <summary>
        /// Gets the entries that fall within the specified time frame.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Returns the entries that fall within the specified time frame.</returns>
        Task<IEnumerable<ResponseEntry>> GetEntries(DateTimeOffset? start = null, DateTimeOffset? end = null);
    }
}