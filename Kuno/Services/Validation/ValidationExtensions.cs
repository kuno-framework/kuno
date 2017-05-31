/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Extends validation classes with additional methods.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Adds the specified type all instances in teh specified collection.
        /// </summary>
        /// <param name="instance">The this instance.</param>
        /// <param name="type">The error type.</param>
        /// <returns>A collection containing all of the errors with the correct type set.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        public static IEnumerable<ValidationError> WithType(this IEnumerable<ValidationError> instance, ValidationType type)
        {
            Argument.NotNull(instance, nameof(instance));

            var instances = instance as ValidationError[] ?? instance.ToArray();

            foreach (var item in instances.Where(e => e.Type == ValidationType.None))
            {
                item.WithType(type);
            }

            return instances;
        }
    }
}