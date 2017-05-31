/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Threading.Tasks;

namespace Kuno.Search
{
    /// <summary>
    /// Defines an interface for rebuilding a search index.
    /// </summary>
    public interface IRebuildSearchIndex
    {
        /// <summary>
        /// Rebuilds the search index.
        /// </summary>
        /// <returns>A task for asynchronous programming.</returns>
        Task RebuildIndexAsync();
    }
}