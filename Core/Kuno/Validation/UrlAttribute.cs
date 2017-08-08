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
    /// Validates that a property is a well-formed URL.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UrlAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlAttribute" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UrlAttribute(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlAttribute" /> class.
        /// </summary>
        public UrlAttribute()
            : base(null)
        {
        }

        /// <inheritdoc />
        public override ValidationError GetValidationError(PropertyInfo property)
        {
            return new ValidationError(this.Code, this.Message ?? property.Name + " must be a well-formed URL.");
        }

        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (value is Uri)
            {
                return true;
            }
            if (value is string)
            {
                var strInput = ((string)value).Trim();
                return Uri.IsWellFormedUriString(strInput, UriKind.RelativeOrAbsolute);
            }
            return false;
        }
    }
}