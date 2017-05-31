/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using Kuno.Reflection;
using Kuno.Services.Logging;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// An event that is raised when state changes within a particular domain.
    /// </summary>
    public class EventMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage" /> class.
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <param name="body">The message body.</param>
        public EventMessage(string requestId, Event body)
            : base(body)
        {
            this.RequestId = requestId;
            this.Name = this.GetEventName();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage" /> class.
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <param name="body">The message body.</param>
        internal EventMessage(string requestId, object body)
            : base(body)
        {
            this.RequestId = requestId;
            this.Name = this.GetEventName();
        }

        /// <summary>
        /// Gets the request message identifier.
        /// </summary>
        /// <value>The request message identifier.</value>
        public string RequestId { get; }

        private string GetEventName()
        {
            var type = this.Body.GetType();
            var attribute = type.GetAllAttributes<EventAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                return attribute.Name;
            }
            return type.Name;
        }
    }
}