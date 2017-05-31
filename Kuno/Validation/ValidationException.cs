/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;

namespace Kuno.Validation
{
    /// <summary>
    /// The exception that is raised when a validation error is found.
    /// </summary>
    /// <seealso cref="System.InvalidOperationException" />
    public class ValidationException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException" /> class.
        /// </summary>
        /// <param name="errors">The validation errors to add to the exception.</param>
        public ValidationException(params ValidationError[] errors)
            : base(string.Join(Environment.NewLine, errors.Select(e => e.Message)))
        {
            this.ValidationErrors = errors;
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>The validation errors.</value>
        public ValidationError[] ValidationErrors { get; }
    }
}