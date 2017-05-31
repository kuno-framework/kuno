/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Performs security validation on a message.
    /// </summary>
    /// <typeparam name="TCommand">The message type.</typeparam>
    public abstract class SecurityRule<TCommand> : ISecurityRule<TCommand>, IUseExecutionContext
    {
        private ExecutionContext _context;

        /// <summary>
        /// Gets the current request.
        /// </summary>
        /// <value>The current request.</value>
        public Request Request => _context?.Request;

        IEnumerable<ValidationError> IValidate<TCommand>.Validate(TCommand instance)
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
            _context = context;
        }

        /// <summary>
        /// Validates the specified message instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        /// <exception cref="System.NotImplementedException">Thrown when neither validate methods are implemented.</exception>
        public virtual ValidationError Validate(TCommand instance)
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
        public virtual Task<ValidationError> ValidateAsync(TCommand instance)
        {
            throw new NotImplementedException();
        }
    }
}