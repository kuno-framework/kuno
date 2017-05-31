/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Kuno.Domain.Serialization;
using Kuno.Validation;

namespace Kuno.Domain
{
    /// <summary>
    /// A <see href="http://bit.ly/2dViCg3">Value Object</see> that can also be represented by another type.
    /// </summary>
    /// <typeparam name="TValue">The type that can be interchangeably used with this concept.</typeparam>
    [JsonConverter(typeof(ConceptConverter))]
    public abstract class ConceptAs<TValue> : IEquatable<ConceptAs<TValue>>, IValidate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptAs{TValue}" /> class.
        /// </summary>
        protected ConceptAs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptAs{TValue}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        protected ConceptAs(TValue value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the concept value.
        /// </summary>
        /// <value>The concept value.</value>
        public TValue Value { get; protected set; }

        /// <summary>
        /// Determines if another instance is equivalent to this instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><c>true</c> if the other instance is equivalent, <c>false</c> otherwise.</returns>
        public virtual bool Equals(ConceptAs<TValue> other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.GetType() == other.GetType() && EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>
        /// Returns validation errors that were found as a result of the validation.
        /// An empty collection means that no errors were found.
        /// </returns>
        public abstract IEnumerable<ValidationError> Validate();

        /// <summary>
        /// Ensures that this instance is valid and throws an exception if not.
        /// </summary>
        /// <exception cref="ValidationException">Thrown when the instance is not valid.</exception>
        public void EnsureValid()
        {
            var results = this.Validate().ToArray();
            if (results.Any())
            {
                throw new ValidationException(results);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj != null && this.Equals(obj as ConceptAs<TValue>);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return GenerateHashForInstance(typeof(TValue), this.Value);
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ConceptAs<TValue> left, ConceptAs<TValue> right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }

            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }

            return (bool) left?.Equals(right);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConceptAs{TValue}" /> to the wrapped value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TValue(ConceptAs<TValue> value)
        {
            return value == null ? default(TValue) : value.Value;
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ConceptAs<TValue> left, ConceptAs<TValue> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.Value == null ? default(TValue) == null ? null : default(TValue)?.ToString() : this.Value.ToString();
        }

        /// <summary>
        /// Convenience method that creates a hash from the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters to create the hash from.</param>
        /// <returns>Returns a hash from the specified parameters.</returns>
        protected static int GenerateHashForInstance(params object[] parameters)
        {
            unchecked
            {
                return parameters.Where(param => param != null).Aggregate(17, (current, param) => current * 29 + param.GetHashCode());
            }
        }
    }
}