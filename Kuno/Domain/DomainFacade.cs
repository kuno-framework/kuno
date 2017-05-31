/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Kuno.Caching;
using Kuno.Validation;

namespace Kuno.Domain
{
    /// <summary>
    /// A default <see cref="IDomainFacade" /> implementation.
    /// </summary>
    /// <seealso cref="IDomainFacade" />
    public class DomainFacade : IDomainFacade
    {
        private readonly ICacheManager _cacheManager;
        private readonly IComponentContext _componentContext;
        private readonly ConcurrentDictionary<Type, object> _instances = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainFacade" /> class.
        /// </summary>
        /// <param name="componentContext">The component request.</param>
        /// <param name="cacheManager">The cache manager.</param>
        public DomainFacade(IComponentContext componentContext, ICacheManager cacheManager)
        {
            Argument.NotNull(componentContext, nameof(componentContext));
            Argument.NotNull(cacheManager, nameof(cacheManager));

            _componentContext = componentContext;
            _cacheManager = cacheManager;
        }

        /// <inheritdoc />
        public async Task Add<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (!instances.Any())
            {
                return;
            }

            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            await repository.Add(instances);
            await _cacheManager.AddAsync(instances);
        }

        /// <inheritdoc />
        public Task Add<TAggregateRoot>(IEnumerable<TAggregateRoot> instances) where TAggregateRoot : IAggregateRoot
        {
            return this.Add(instances.ToArray());
        }

        /// <inheritdoc />
        public Task Add<TAggregateRoot>(List<TAggregateRoot> instances) where TAggregateRoot : IAggregateRoot
        {
            return this.Add(instances.ToArray());
        }

        /// <inheritdoc />
        public async Task Clear<TAggregateRoot>() where TAggregateRoot : IAggregateRoot
        {
            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            await repository.Clear();

            await _cacheManager.ClearAsync();
        }

        /// <inheritdoc />
        public Task<bool> Exists<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> expression) where TAggregateRoot : IAggregateRoot
        {
            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            return repository.Exists(expression);
        }

        /// <inheritdoc />
        public async Task<TAggregateRoot> Find<TAggregateRoot>(string id) where TAggregateRoot : IAggregateRoot
        {
            var target = await _cacheManager.FindAsync<TAggregateRoot>(id);
            if (target != null)
            {
                return target;
            }

            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            target = await repository.Find(id);

            if (target != null)
            {
                await _cacheManager.AddAsync(target);
            }

            return target;
        }

        /// <inheritdoc />
        public Task<IEnumerable<TAggregateRoot>> Find<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> expression) where TAggregateRoot : IAggregateRoot
        {
            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            return repository.Find(expression);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TAggregateRoot>> Find<TAggregateRoot>() where TAggregateRoot : IAggregateRoot
        {
            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            return repository.Find();
        }

        /// <inheritdoc />
        public async Task Remove<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (!instances.Any())
            {
                return;
            }

            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            await repository.Remove(instances);

            await _cacheManager.RemoveAsync(instances);
        }

        /// <inheritdoc />
        public Task Remove<TAggregateRoot>(IEnumerable<TAggregateRoot> instances) where TAggregateRoot : IAggregateRoot
        {
            return this.Remove(instances.ToArray());
        }

        /// <inheritdoc />
        public Task Remove<TAggregateRoot>(List<TAggregateRoot> instances) where TAggregateRoot : IAggregateRoot
        {
            return this.Remove(instances.ToArray());
        }

        /// <inheritdoc />
        public async Task Update<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (!instances.Any())
            {
                return;
            }

            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            await repository.Update(instances);

            await _cacheManager.UpdateAsync(instances);
        }

        /// <inheritdoc />
        public Task Update<TAggregateRoot>(IEnumerable<TAggregateRoot> instances) where TAggregateRoot : IAggregateRoot
        {
            return this.Update(instances.ToArray());
        }

        /// <inheritdoc />
        public Task Update<TAggregateRoot>(List<TAggregateRoot> instances) where TAggregateRoot : IAggregateRoot
        {
            return this.Update(instances.ToArray());
        }

        /// <inheritdoc />
        public Task<bool> Exists<TAggregateRoot>(string id) where TAggregateRoot : IAggregateRoot
        {
            var repository = (IRepository<TAggregateRoot>) _instances.GetOrAdd(typeof(TAggregateRoot), t => _componentContext.Resolve<IRepository<TAggregateRoot>>());

            if (repository == null)
            {
                throw new InvalidOperationException($"No repository has been registered for type {typeof(TAggregateRoot)}.");
            }

            return repository.Exists(id);
        }
    }
}