using System;
using System.Reflection;

namespace Kuno.Validation
{
    /// <summary>
    /// Validates that a property is not null or whitespace.
    /// </summary>
    /// <seealso cref="Kuno.Validation.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullOrWhiteSpaceAttribute" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotNullOrWhiteSpaceAttribute(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullOrWhiteSpaceAttribute" /> class.
        /// </summary>
        public NotNullOrWhiteSpaceAttribute()
            : base(null)
        {
        }

        /// <inheritdoc />
        public override ValidationError GetValidationError(PropertyInfo property)
        {
            return new ValidationError(this.Code, this.Message ?? property.Name + " must be specified.");
        }

        /// <summary>
        /// Returns true if the object value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid(object value)
        {
            return !string.IsNullOrWhiteSpace(value as string);
        }
    }
}