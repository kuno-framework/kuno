using System;
using System.Reflection;

namespace Kuno.Validation
{
    /// <summary>
    /// Base class for validation attributes.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public abstract class ValidationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttribute" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        protected ValidationAttribute(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; private set; }

        /// <summary>
        /// Returns true if the object value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        public abstract bool IsValid(object value);

        /// <summary>
        /// Gets the validation error for the specified property.
        /// </summary>
        /// <param name="property">The property being validated.</param>
        /// <returns>Returns the validation error for the specified property.</returns>
        public virtual ValidationError GetValidationError(PropertyInfo property)
        {
            return new ValidationError(this.Code, this.Message);
        }
    }
}