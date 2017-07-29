/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Validation
{
    /// <summary>
    /// Validates that a property is greater than another value.
    /// </summary>
    /// <seealso cref="Kuno.Validation.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterThanAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullOrWhiteSpaceAttribute" /> class.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="message">The message.</param>
        public GreaterThanAttribute(int value, string message)
            : base(message)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value to compare.
        /// </summary>
        /// <value>The value to compare.</value>
        public int Value { get; private set; }

        /// <summary>
        /// Returns true if the object value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid(object value)
        {
            return value is int && (int) value > this.Value;
        }
    }
}