/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Kuno.Validation
{
    /// <summary>
    /// Validates that a property is valid JSON.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAttribute" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public JsonAttribute(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAttribute" /> class.
        /// </summary>
        public JsonAttribute()
            : base(null)
        {
        }

        /// <inheritdoc />
        public override ValidationError GetValidationError(PropertyInfo property)
        {
            return new ValidationError(this.Code, this.Message ?? property.Name + " must be a valid JSON.");
        }

        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                var strInput = ((string)value).Trim();
                if (strInput.StartsWith("{") && strInput.EndsWith("}") || strInput.StartsWith("[") && strInput.EndsWith("]"))
                {
                    try
                    {
                        JToken.Parse(strInput);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}