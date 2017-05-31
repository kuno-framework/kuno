/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kuno.Search
{
    /// <summary>
    /// Defines a search requestContext that is used to access a search source.
    /// </summary>
    public interface ISearchContext
    {
        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TSearchResult">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        /// <returns>A task for asynchronous programming.</returns>
        Task AddAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult;

        /// <summary>
        /// Clears all instances of the specified type.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <returns>A task for asynchronous programming.</returns>
        Task ClearAsync<TSearchResult>() where TSearchResult : class, ISearchResult;

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task<TSearchResult> FindAsync<TSearchResult>(int id) where TSearchResult : class, ISearchResult;

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        /// <returns>A task for asynchronous programming.</returns>
        Task RemoveAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult;

        /// <summary>
        /// Removes the instances that match the specified predicate.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to remove.</typeparam>
        /// <param name="predicate">The predicate used to filter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate" /> argument is null.</exception>
        /// <returns>A task for asynchronous programming.</returns>
        Task RemoveAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate) where TSearchResult : class, ISearchResult;

        /// <summary>
        /// Opens a query that can be used to filter and project.
        /// </summary>
        /// <param name="text">The text to use for search.</param>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TAggregateRoot&gt; that can be used to filter and project.</returns>
        IQueryable<TSearchResult> Search<TSearchResult>(string text = null) where TSearchResult : class, ISearchResult;

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        /// <returns>A task for asynchronous programming.</returns>
        Task UpdateAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult;

        /// <summary>
        /// Updates the specified instances using the specified predicate and update expression.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <param name="predicate">The predicate used to filter.</param>
        /// <param name="expression">The expression used to update.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate" /> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="expression" /> argument is null.</exception>
        Task UpdateAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate, Expression<Func<Type, Type>> expression) where TSearchResult : class, ISearchResult;
    }
}