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
using Kuno.Domain;
using Kuno.Logging;
using Kuno.Validation;

namespace Kuno.Search
{
    /// <summary>
    /// Manages a search index with methods to immediately add an item to the index.
    /// </summary>
    /// <typeparam name="TSearchResult">The type of the search result.</typeparam>
    /// <seealso cref="ISearchIndex{TSearchResult}" />
    public class SearchIndex<TSearchResult> : ISearchIndex<TSearchResult> where TSearchResult : class, ISearchResult
    {
        private readonly ISearchContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchIndex{TSearchResult}" /> class.
        /// </summary>
        /// <param name="context">The configured requestContext.</param>
        public SearchIndex(ISearchContext context)
        {
            Argument.NotNull(context, nameof(context));

            _context = context;
        }

        /// <summary>
        /// Gets or sets the configured <see cref="IDomainFacade" />.
        /// </summary>
        /// <value>The configured <see cref="IDomainFacade" />.</value>
        public IDomainFacade Domain { get; set; }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger" />.
        /// </summary>
        /// <value>The configured <see cref="ILogger" />.</value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Adds the specified instances to the index immediately. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <param name="instances">The instances to add immediately.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public virtual Task AddAsync(params TSearchResult[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            this.Logger.Verbose($"Adding {instances.Count()} items of type {typeof(TSearchResult)} using {_context.GetType()}.");

            return _context.AddAsync(instances);
        }

        /// <summary>
        /// Clears all instances of the specified type.
        /// </summary>
        /// <returns>A task for asynchronous programming.</returns>
        public virtual Task ClearAsync()
        {
            this.Logger.Verbose($"Clearing all items of type {typeof(TSearchResult)} using {_context.GetType()}.");

            return _context.ClearAsync<TSearchResult>();
        }

        /// <summary>
        /// Opens a query that can be used to filter and project.
        /// </summary>
        /// <param name="text">The text to use for search.</param>
        /// <returns>An IQueryable&lt;TSearchResult&gt; that can be used to filter and project.</returns>
        public virtual IQueryable<TSearchResult> Search(string text = null)
        {
            this.Logger.Verbose($"Opening a query for type {typeof(TSearchResult)} using {_context.GetType()}.");

            return _context.Search<TSearchResult>(text);
        }

        /// <summary>
        /// Removes all instances that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public virtual Task RemoveAsync(Expression<Func<TSearchResult, bool>> predicate)
        {
            this.Logger.Verbose($"Removing items of type {typeof(TSearchResult)} using {_context.GetType()}.");

            return _context.RemoveAsync(predicate);
        }

        /// <inheritdoc />
        public virtual Task Index(params string[] ids)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public virtual Task RemoveAsync(params TSearchResult[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            this.Logger.Verbose($"Removing {instances.Count()} items of type {typeof(TSearchResult)} using {_context.GetType()}.");

            return _context.RemoveAsync(instances);
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        public virtual Task<TSearchResult> FindAsync(int id)
        {
            this.Logger.Verbose($"Finding item of type {typeof(TSearchResult)} with ID {id} using {_context.GetType()}.");

            return _context.FindAsync<TSearchResult>(id);
        }

        /// <summary>
        /// Rebuilds the search index.
        /// </summary>
        /// <returns>A task for asynchronous programming.</returns>
        public virtual Task RebuildIndexAsync()
        {
            this.Logger.Verbose($"Rebuilding index for items of type {typeof(TSearchResult)} using {_context.GetType()}.");

            return Task.FromResult(0);
        }

        /// <summary>
        /// Updates the specified instances immediately. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <param name="instances">The instances to update immediately.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public virtual Task UpdateAsync(params TSearchResult[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            this.Logger.Verbose($"Updating {instances.Count()} items of type {typeof(TSearchResult)} using {_context.GetType()}.");

            return _context.UpdateAsync(instances);
        }

        /// <summary>
        /// Updates all instances found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update to make.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual Task UpdateAsync(Expression<Func<TSearchResult, bool>> predicate, Expression<Func<TSearchResult, TSearchResult>> expression)
        {
            throw new NotSupportedException();
        }
    }
}