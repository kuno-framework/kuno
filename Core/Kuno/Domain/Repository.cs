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
using System.Threading.Tasks;
using Kuno.Logging;
using Kuno.Validation;

namespace Kuno.Domain
{
    /// <summary>
    /// A default <see href="http://bit.ly/2dVQsXu">Repository</see> for an <see cref="IAggregateRoot" />.
    /// </summary>
    /// <typeparam name="TRoot">The type of <see cref="IAggregateRoot" />.</typeparam>
    /// <seealso href="http://bit.ly/2dVQsXu">Domain-Driven Design: Tackling Complexity in the Heart of Software</seealso>
    public class Repository<TRoot> : IRepository<TRoot> where TRoot : class, IAggregateRoot
    {
        private readonly IEntityContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TRoot}" /> class.
        /// </summary>
        /// <param name="context">The configured requestContext.</param>
        public Repository(IEntityContext context)
        {
            Argument.NotNull(context, nameof(context));

            _context = context;
        }

        /// <inheritdoc />
        public ILogger Logger { get; set; }

        /// <inheritdoc />
        public Task Add(TRoot[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            this.Logger.Verbose($"Adding {instances.Count()} items of type {typeof(TRoot)} using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Add(instances);
        }

        /// <inheritdoc />
        public virtual Task Clear()
        {
            this.Logger.Verbose($"Clearing all items of type {typeof(TRoot)} using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Clear<TRoot>();
        }

        /// <inheritdoc />
        public virtual Task<bool> Exists(Expression<Func<TRoot, bool>> expression)
        {
            Argument.NotNull(expression, nameof(expression));

            this.Logger.Verbose($"Checking to see if items of type {typeof(TRoot)} exist using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Exists(expression);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TRoot>> Find(Expression<Func<TRoot, bool>> expression)
        {
            Argument.NotNull(expression, nameof(expression));

            this.Logger.Verbose($"Finding items of type {typeof(TRoot)} using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Find(expression);
        }

        /// <inheritdoc />
        public virtual Task<TRoot> Find(string id)
        {
            this.Logger.Verbose($"Finding item of type {typeof(TRoot)} with ID {id} using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Find<TRoot>(id);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TRoot>> Find()
        {
            this.Logger.Verbose($"Finding all items of type {typeof(TRoot)} using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Find<TRoot>();
        }

        /// <inheritdoc />
        public virtual Task Remove(TRoot[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            this.Logger.Verbose($"Removing {instances.Count()} items of type {typeof(TRoot)} using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Remove(instances);
        }

        /// <inheritdoc />
        public virtual Task Update(TRoot[] instances)
        {
            Argument.NotNull(instances, nameof(instances));

            this.Logger.Verbose($"Updating {instances.Count()} items of type {typeof(TRoot)} using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Update(instances);
        }

        /// <inheritdoc />
        public virtual Task<bool> Exists(string id)
        {
            Argument.NotNullOrWhiteSpace(id, nameof(id));

            this.Logger.Verbose($"Checking to see if items of type {typeof(TRoot)} exist using {this.GetType().Name}:{_context.GetType().Name}.");

            return _context.Exists<TRoot>(id);
        }
    }
}