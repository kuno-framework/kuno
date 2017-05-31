/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kuno.Validation
{
    /// <summary>
    /// Provides consistent validation routines and messaging.
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Validates that the argument is not null.
        /// </summary>
        public static void NotNull(object instance, string name)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Validates that the argument is not null or empty.
        /// </summary>
        public static void NotNullOrEmpty<T>(IEnumerable<T> value, string name)
        {
            if (value == null || !value.Any())
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "The argument {0} must be not be null and must have values.", name), name);
            }
        }

        /// <summary>
        /// Validates that the argument is not null or whitespace.
        /// </summary>
        public static void NotNullOrWhiteSpace(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "The argument {0} must be a non-empty value.  The passed in value is \"{1}\".", name, value), name);
            }
        }
    }
}