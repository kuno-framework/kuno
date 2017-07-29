/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// An atomic packet of data that is transmitted through a messaging channel.
    /// </summary>
    public class MessageEnvelope : IEquatable<MessageEnvelope>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEnvelope" /> class.
        /// </summary>
        /// <param name="body">The message body.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="body"/> is null.  If a message without a body is desired, please use the parameterless constructor.</exception>
        public MessageEnvelope(object body)
        {
            this.Body = body ?? throw new ArgumentNullException(nameof(body), "Argument 'body' cannot be null.  If a message without a body is desired, please use the parameterless constructor.");

            var type = body.GetType();
            this.MessageType = type;
            this.Name = type.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEnvelope" /> class.
        /// </summary>
        public MessageEnvelope()
        {
        }

        /// <summary>
        /// Gets the message body.
        /// </summary>
        /// <value>The body or content of the message.</value>
        public object Body { get; private set; }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>The identifier of the message.</value>
        public string Id { get; private set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the message type.
        /// </summary>
        /// <value>The type of the message.</value>
        public Type MessageType { get; private set; }

        /// <summary>
        /// Gets the message name.
        /// </summary>
        /// <value>The name of the message.</value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the message timestamp.
        /// </summary>
        /// <value>When the message was created.</value>
        public DateTimeOffset TimeStamp { get; private set; } = DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public bool Equals(MessageEnvelope other)
        {
            return string.Equals(this.Id, other?.Id);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((MessageEnvelope)obj);
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return (this.Id != null ? this.Id.GetHashCode() : 0);
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>Returns the result of the operator.</returns>
        public static bool operator ==(MessageEnvelope x, MessageEnvelope y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>Returns the result of the operator.</returns>
        public static bool operator !=(MessageEnvelope x, MessageEnvelope y)
        {
            return !(x == y);
        }
    }
}