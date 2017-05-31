/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Validates an object instance based on input rules.
    /// </summary>
    /// <typeparam name="TCommand">The type of message.</typeparam>
    public abstract class InputRule<TCommand> : IInputRule<TCommand>
    {
        /// <summary>
        /// Validates the specified message instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        public abstract IEnumerable<ValidationError> Validate(TCommand instance);
    }
}