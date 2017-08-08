// Copyright (c) Kuno Contributors
// 
// This file is subject to the terms and conditions defined in
// the LICENSE file, which is part of this source code package.

using System;
using Kuno.Serialization;
using Kuno.Utilities.NewId;
using Kuno.Validation;
using Newtonsoft.Json;

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

            if (type == typeof(string))
            {
                this.Body = (string)body;
            }
            else
            {
                this.Body = JsonConvert.SerializeObject(body, DefaultSerializationSettings.Instance);
            }
            this.MessageType = type.FullName;
            this.Name = type.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        public Message()
        {
        }

        /// <inheritdoc />
        public string Id { get; private set; } = NewId.NextId();

        /// <inheritdoc />
        public DateTimeOffset TimeStamp { get; private set; } = DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public string Body { get; private set; }

        /// <inheritdoc />
        public string MessageType { get; private set; }

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