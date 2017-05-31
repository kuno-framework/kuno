/* 
 * Copyright (c) Stacks Contributors
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
    /// Defines an entity context that is used to access a single data source.
    /// </summary>
    public interface IEntityContext
    {
        /// <summary>
        /// Adds the specified instances.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        Task Add<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Clears all instances.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>A task for asynchronous programming.</returns>
        Task Clear<TEntity>() where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Determines if any instances with the specified expression exist.
        /// </summary>
        /// <param name="expression">The expression to filter with.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Determines if an instance with the specified ID exists.
        /// </summary>
        /// <typeparam name="TEntity">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<bool> Exists<TEntity>(string id) where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<TEntity> Find<TEntity>(string id) where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression to filter with.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<IEnumerable<TEntity>> Find<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Finds all instances of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>A task for asynchronous programming.</returns>
        Task<IEnumerable<TEntity>> Find<TEntity>() where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        Task Remove<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot;

        /// <summary>
        /// Updates the specified instances.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        Task Update<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot;
    }
}