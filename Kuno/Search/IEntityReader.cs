/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;

namespace Kuno.Search
{
    /// <summary>
    /// Reads data elements from the domain so that they can be indexed.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to read.</typeparam>
    public interface IEntityReader<TEntity>
    {
        /// <summary>
        /// Reads elements so that they can be indexed.
        /// </summary>
        /// <returns>Returns a query that can be used to layer additional queries.</returns>
        IQueryable<TEntity> Read();
    }
}