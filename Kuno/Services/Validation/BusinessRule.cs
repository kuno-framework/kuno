/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kuno.Domain;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Base class for business validation rule.
    /// </summary>
    /// <typeparam name="TValue">The message type.</typeparam>
    /// <seealso cref="Kuno.Services.Validation.IBusinessRule{TValue}" />
    /// <seealso cref="IBusinessRule{TCommand}" />
    public abstract class BusinessRule<TValue> : IBusinessRule<TValue>, IUseExecutionContext
    {
        /// <summary>
        /// Gets the configured <see cref="IDomainFacade" />.
        /// </summary>
        /// <value>The configured <see cref="IDomainFacade" />.</value>
        public IDomainFacade Domain { get; protected set; }

        /// <summary>
        /// Gets the current request.
        /// </summary>
        /// <value>The current frequest.</value>
        public Request Request => this.Context?.Request;

        /// <summary>
        /// Gets or sets the current execution context.
        /// </summary>
        /// <value>The current execution context.</value>
        internal ExecutionContext Context { get; set; }

        IEnumerable<ValidationError> IValidate<TValue>.Validate(TValue instance)
        {
            var target = this.Validate(instance);
            if (target == null)
            {
                return Enumerable.Empty<ValidationError>();
            }
            return new[] {target};
        }

        void IUseExecutionContext.UseContext(ExecutionContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Validates the specified message instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        /// <exception cref="System.NotImplementedException">Thrown when neither validate methods are implemented.</exception>
        public virtual ValidationError Validate(TValue instance)
        {
            Argument.NotNull(instance, nameof(instance));

            return this.ValidateAsync(instance).Result;
        }


        /// <summary>
        /// Validates the specified message instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="System.NotImplementedException">Thrown when neither validate methods are implemented.</exception>
        public virtual Task<ValidationError> ValidateAsync(TValue instance)
        {
            throw new NotImplementedException();
        }
    }
}