/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */
 
using System;
using System.Reflection;

namespace Kuno.Validation
{
    /// <summary>
    /// Validates that a property is not null.
    /// </summary>
    /// <seealso cref="Kuno.Validation.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullAttribute" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotNullAttribute(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullAttribute" /> class.
        /// </summary>
        public NotNullAttribute()
            : base(null)
        {
        }

        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            return value != null;
        }

        /// <inheritdoc />
        public override ValidationError GetValidationError(PropertyInfo property)
        {
            return new ValidationError(this.Code, this.Message ?? property.Name + " must be specified.");
        }
    }
}