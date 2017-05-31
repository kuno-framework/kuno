/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Kuno.Validation;

namespace Kuno.Search
{
    /// <summary>
    /// An in-memory <see cref="ISearchContext" /> instance.
    /// </summary>
    /// <seealso cref="Kuno.Search.ISearchContext" />
    public class InMemorySearchContext : ISearchContext
    {
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private readonly List<ISearchResult> _instances = new List<ISearchResult>();
        private int _index;

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        public Task AddAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            Argument.NotNull(instances, nameof(instances));

            _cacheLock.EnterWriteLock();
            try
            {
                foreach (var instance in instances)
                {
                    instance.Id = _index++;
                    _instances.Add(instance);
                }
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Clears all instances of the specified type.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <returns>A task for asynchronous programming.</returns>
        public Task ClearAsync<TSearchResult>() where TSearchResult : class, ISearchResult
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _instances.RemoveAll(e => e is TSearchResult);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Opens a query that can be used to filter and project.
        /// </summary>
        /// <param name="text">The text to use for search.</param>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TAggregateRoot&gt; that can be used to filter and project.</returns>
        public IQueryable<TSearchResult> Search<TSearchResult>(string text = null) where TSearchResult : class, ISearchResult
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return _instances.OfType<TSearchResult>().AsQueryable().Contains(text);
                }
                return _instances.OfType<TSearchResult>().AsQueryable();
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task RemoveAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            _cacheLock.EnterWriteLock();
            try
            {
                var ids = instances.Select(e => e.Id).ToList();
                _instances.RemoveAll(e => ids.Contains(e.Id));
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Removes the instances that match the specified predicate.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to remove.</typeparam>
        /// <param name="predicate">The predicate used to filter.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task RemoveAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate) where TSearchResult : class, ISearchResult
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _instances.RemoveAll(e => predicate.Compile()((TSearchResult) e));
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task<TSearchResult> FindAsync<TSearchResult>(int id) where TSearchResult : class, ISearchResult
        {
            _cacheLock.EnterReadLock();
            try
            {
                return Task.FromResult((TSearchResult) _instances.Find(e => e.Id == id));
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
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
        public async Task UpdateAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            await this.RemoveAsync(instances);

            await this.AddAsync(instances);
        }

        /// <summary>
        /// Updates the specified instances using the specified predicate and update expression.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <param name="predicate">The predicate used to filter.</param>
        /// <param name="expression">The expression used to update.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task UpdateAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate, Expression<Func<Type, Type>> expression) where TSearchResult : class, ISearchResult
        {
            throw new NotSupportedException();
        }
    }
}