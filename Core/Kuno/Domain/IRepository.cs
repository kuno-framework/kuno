/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kuno.Domain
{
    /// <summary>
    /// Defines a <see href="http://bit.ly/2dVQsXu">Repository</see> for an <see cref="IAggregateRoot" />.
    /// </summary>
    /// <typeparam name="TRoot">The type of <see cref="IAggregateRoot" />.</typeparam>
    /// <seealso href="http://bit.ly/2dVQsXu">Domain-Driven Design: Tackling Complexity in the Heart of Software</seealso>
    public interface IRepository<TRoot> where TRoot : IAggregateRoot
    {
        /// <summary>
        /// Adds the specified instances.
        /// </summary>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        Task Add(TRoot[] instances);

        /// <summary>
        /// Clears all instances.
        /// </summary>
        /// <returns>A task for asynchronous programming.</returns>
        Task Clear();

        /// <summary>
        /// Determines if any instances with the specified expression exist.
        /// </summary>
        /// <param name="expression">The expression to filter with.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<bool> Exists(Expression<Func<TRoot, bool>> expression);

        /// <summary>
        /// Determines if an instance with the specified ID exists.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<bool> Exists(string id);

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<TRoot> Find(string id);

        /// <summary>
        /// Finds instances with the specified expression.
        /// </summary>
        /// <param name="expression">The expression to filter with.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<IEnumerable<TRoot>> Find(Expression<Func<TRoot, bool>> expression);

        /// <summary>
        /// Finds all instances.
        /// </summary>
        /// <returns>A task for asynchronous programming.</returns>
        Task<IEnumerable<TRoot>> Find();

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        /// <returns>A task for asynchronous programming.</returns>
        Task Remove(TRoot[] instances);

        /// <summary>
        /// Updates the specified instances.
        /// </summary>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        Task Update(TRoot[] instances);
    }
}