/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Kuno.Utilities.NewId;
using Kuno.Validation;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// An atomic packet of data that is transmitted through a message channel.
    /// </summary>
    public class Message : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="body">The message body.</param>
        public Message(object body)
        {
            Argument.NotNull(body, nameof(body));

            var type = body.GetType();

            this.Body = body;
            this.MessageType = type;
            this.Name = type.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        public Message()
        {
        }

        /// <inheritdoc />
        public string Id { get; } = NewId.NextId();

        /// <inheritdoc />
        public DateTimeOffset TimeStamp { get; } = DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public object Body { get; }

        /// <inheritdoc />
        public Type MessageType { get; }

        /// <inheritdoc />
        public string Name { get; protected set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return this.Id == (obj as IMessage)?.Id;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>Returns the result of the operator.</returns>
        public static bool operator ==(Message x, Message y)
        {
            return ReferenceEquals(x, y) || x.Equals(y);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>Returns the result of the operator.</returns>
        public static bool operator !=(Message x, Message y)
        {
            return !(x == y);
        }
    }
}