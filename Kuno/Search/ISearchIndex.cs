/* 
 * Copyright (c) Stacks Contributors
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
    /// Manages a search index with methods to immediately add an item to the index.
    /// </summary>
    public interface ISearchIndex<TSearchResult> : IRebuildSearchIndex where TSearchResult : class, ISearchResult
    {
        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <param name="instances">The instances to add.</param>
        Task AddAsync(TSearchResult[] instances);

        /// <summary>
        /// Clears all instances of the specified type.
        /// </summary>
        Task ClearAsync();

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        Task<TSearchResult> FindAsync(int id);

        /// <summary>
        /// Indexes the the items with the specified IDs.
        /// </summary>
        /// <param name="ids">The IDs of the items to index.</param>
        /// <returns>Retuns a task for asynchronous programming.</returns>
        Task Index(params string[] ids);

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        Task RemoveAsync(TSearchResult[] instances);

        /// <summary>
        /// Removes all instances that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate" /> argument is null.</exception>
        Task RemoveAsync(Expression<Func<TSearchResult, bool>> predicate);

        /// <summary>
        /// Opens a query that can be used to filter and project.
        /// </summary>
        /// <param name="text">The text to use for search.</param>
        /// <returns>An IQueryable&lt;TSearchResult&gt; that can be used to filter and project.</returns>
        IQueryable<TSearchResult> Search(string text = null);

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        Task UpdateAsync(TSearchResult[] instances);

        /// <summary>
        /// Updates all instances found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update to make.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate" /> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="expression" /> argument is null.</exception>
        Task UpdateAsync(Expression<Func<TSearchResult, bool>> predicate, Expression<Func<TSearchResult, TSearchResult>> expression);
    }
}