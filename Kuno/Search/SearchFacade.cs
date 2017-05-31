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
using Autofac;
using Kuno.Validation;

namespace Kuno.Search
{
    /// <summary>
    /// Provides a single access point to instances, allows for search stores to be granular and for
    /// application/infrastructure components to access objects with minimal bloat and lifetime management;  Instead of using
    /// many dependencies, in each class, for each data access component, the facade can be used and it will resolve the
    /// dependences as needed instead of on construction.
    /// </summary>
    public class SearchFacade : ISearchFacade
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFacade" /> class.
        /// </summary>
        /// <param name="componentContext">The component requestContext.</param>
        public SearchFacade(IComponentContext componentContext)
        {
            Argument.NotNull(componentContext, nameof(componentContext));

            _componentContext = componentContext;
        }

        /// <summary>
        /// Reads items from the domain.  To be used when indexing.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>Returns a query to index.</returns>
        public IQueryable<TEntity> Read<TEntity>()
        {
            var target = _componentContext.ResolveOptional<IEntityReader<TEntity>>();

            if (target == null)
            {
                throw new InvalidOperationException($"No reader has been registered for type {typeof(TEntity)}.");
            }

            return target.Read();
        }

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception>Thrown when the <paramref name="instances" /> argument is null.</exception>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        public Task AddAsync<TSearchResult>(params TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            Argument.NotNull(instances, nameof(instances));

            if (!instances.Any())
            {
                return Task.FromResult(0);
            }

            var store = _componentContext.ResolveOptional<ISearchIndex<TSearchResult>>();

            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }

            return store.AddAsync(instances);
        }

        /// <summary>
        /// Clears all instances of the specified type.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <returns>A task for asynchronous programming.</returns>
        public Task ClearAsync<TSearchResult>() where TSearchResult : class, ISearchResult
        {
            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();

            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }

            return store.ClearAsync();
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        public Task<TSearchResult> FindAsync<TSearchResult>(int id) where TSearchResult : class, ISearchResult
        {
            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }
            return store.FindAsync(id);
        }

        /// <summary>
        /// Rebuilds the index of the specified search result type.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of search result.</typeparam>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task RebuildIndexAsync<TSearchResult>() where TSearchResult : class, ISearchResult
        {
            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }
            return store.RebuildIndexAsync();
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception>Thrown when the <paramref name="instances" /> argument is null.</exception>
        public Task RemoveAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            Argument.NotNull(instances, nameof(instances));

            if (!instances.Any())
            {
                return Task.FromResult(0);
            }

            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }
            return store.RemoveAsync(instances);
        }

        /// <summary>
        /// Removes all instances that match the specified predicate.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception>Thrown when the <paramref name="predicate" /> argument is null.</exception>
        public Task RemoveAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate) where TSearchResult : class, ISearchResult
        {
            Argument.NotNull(predicate, nameof(predicate));

            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }
            return store.RemoveAsync(predicate);
        }

        /// <summary>
        /// Creates a query that can be used to search.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <param name="text">The text to use for search.</param>
        /// <returns>An IQueryable&lt;TSearchResult&gt; that can be used to filter and project.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public IQueryable<TSearchResult> Search<TSearchResult>(string text = null) where TSearchResult : class, ISearchResult
        {
            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No search indexer has been registered for type {typeof(TSearchResult)}.");
            }
            return store.Search(text);
        }

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <exception>Thrown when the <paramref name="instances" /> argument is null.</exception>
        public Task UpdateAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            Argument.NotNull(instances, nameof(instances));

            if (!instances.Any())
            {
                return Task.FromResult(0);
            }

            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }
            return store.UpdateAsync(instances);
        }

        /// <summary>
        /// Updates all instances found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update make.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception>Thrown when the <paramref name="predicate" /> argument is null.</exception>
        /// <exception>Thrown when the <paramref name="expression" /> argument is null.</exception>
        public Task UpdateAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate, Expression<Func<TSearchResult, TSearchResult>> expression) where TSearchResult : class, ISearchResult
        {
            Argument.NotNull(predicate, nameof(predicate));
            Argument.NotNull(expression, nameof(expression));

            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }
            return store.UpdateAsync(predicate, expression);
        }


        /// <summary>
        /// Indexes the the items with the specified IDs.
        /// </summary>
        /// <param name="ids">The IDs of the items to index.</param>
        /// <returns>Retuns a task for asynchronous programming.</returns>
        public Task Index<TSearchResult>(params string[] ids) where TSearchResult : class, ISearchResult
        {
            var store = _componentContext.Resolve<ISearchIndex<TSearchResult>>();
            if (store == null)
            {
                throw new InvalidOperationException($"No index has been registered for type {typeof(TSearchResult)}.");
            }
            return store.Index(ids);
        }
    }
}