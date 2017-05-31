/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kuno.Validation;

namespace Kuno.Caching
{
    /// <summary>
    /// A local <see cref="ICacheManager" /> implementation that uses an in-memory store.  This is not to be used in a distributed
    /// environment.
    /// </summary>
    /// <seealso cref="Kuno.Caching.ICacheManager" />
    public class LocalCacheManager : ICacheManager
    {
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private readonly List<object> _instances = new List<object>();

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public int ItemCount => _instances.Count;

        /// <summary>
        /// Adds the items to the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of items to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        public virtual Task AddAsync<TItem>(params TItem[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            _cacheLock.EnterWriteLock();
            try
            {
                foreach (var instance in instances)
                {
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
        /// Clears the cache.
        /// </summary>
        /// <returns>Returns a task for asynchronous programming.</returns>
        public Task ClearAsync()
        {
            _instances.Clear();

            return Task.FromResult(0);
        }

        /// <summary>
        /// Finds the item in the cache with the specified ID.
        /// </summary>
        /// <typeparam name="TItem">The type of item to find.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        public virtual Task<TItem> FindAsync<TItem>(string id)
        {
            _cacheLock.EnterReadLock();
            try
            {
                return Task.FromResult((TItem) _instances.Find(e => ItemIdentity.GetIdentity(e) == id));
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Removes the specified items to the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of items to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        public virtual Task RemoveAsync<TItem>(params TItem[] instances)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                var ids = instances.Select(e => ItemIdentity.GetIdentity(e)).ToList();
                _instances.RemoveAll(e => ids.Contains(ItemIdentity.GetIdentity(e)));
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Updates the items in the cache.
        /// </summary>
        /// <typeparam name="TItem">The type of items to update.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        public virtual async Task UpdateAsync<TItem>(params TItem[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            await this.RemoveAsync(instances);

            await this.AddAsync(instances);
        }

        /// <summary>
        /// Removes the items with the specified keys.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        public virtual Task RemoveAsync(params string[] keys)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _instances.RemoveAll(e => keys.Contains(ItemIdentity.GetIdentity(e)));
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }
    }
}