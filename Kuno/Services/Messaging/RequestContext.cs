/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using Kuno.Reflection;
using Kuno.Services.Inventory;
using Kuno.Utilities.NewId;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// The default request context.
    /// </summary>
    public class RequestContext : IRequestContext
    {
        private string _sessionId;

        /// <inheritdoc />
        public Request Resolve(object message, EndPointMetaData endPoint, Request parent = null)
        {
            return new Request
            {
                CorrelationId = parent?.CorrelationId ?? this.GetCorrelationId(),
                SourceAddress = parent?.SourceAddress ?? this.GetSourceIPAddress(),
                SessionId = parent?.SessionId ?? this.GetSession(),
                User = parent?.User ?? this.GetUser(),
                Parent = parent,
                Path = endPoint.Path,
                Message = this.GetMessage(message, endPoint)
            };
        }

        /// <inheritdoc />
        public Request Resolve(string path, object message, Request parent = null)
        {
            return new Request
            {
                CorrelationId = parent?.CorrelationId ?? this.GetCorrelationId(),
                SourceAddress = parent?.SourceAddress ?? this.GetSourceIPAddress(),
                SessionId = parent?.SessionId ?? this.GetSession(),
                User = parent?.User ?? this.GetUser(),
                Parent = parent,
                Path = path ?? message?.GetType().GetAllAttributes<RequestAttribute>().FirstOrDefault()?.Path,
                Message = this.GetMessage(message)
            };
        }

        /// <inheritdoc />
        public Request Resolve(string command, EndPointMetaData endPoint, Request parent = null)
        {
            return new Request
            {
                CorrelationId = parent?.CorrelationId ?? this.GetCorrelationId(),
                SourceAddress = parent?.SourceAddress ?? this.GetSourceIPAddress(),
                SessionId = parent?.SessionId ?? this.GetSession(),
                User = parent?.User ?? this.GetUser(),
                Parent = parent,
                Path = endPoint.Path,
                Message = this.GetMessage(command, endPoint)
            };
        }

        /// <inheritdoc />
        public Request Resolve(EventMessage instance, Request parent)
        {
            return new Request
            {
                CorrelationId = parent.CorrelationId,
                SourceAddress = parent.SourceAddress,
                SessionId = parent.SessionId,
                User = parent.User,
                Parent = parent,
                Message = instance
            };
        }

        /// <summary>
        /// Gets the current correlation ID.
        /// </summary>
        /// <returns>Returns the current correlation ID.</returns>
        protected virtual string GetCorrelationId()
        {
            return NewId.NextId();
        }

        /// <summary>
        /// Gets the current session ID.
        /// </summary>
        /// <returns>Returns the current session ID.</returns>
        protected virtual string GetSession()
        {
            return _sessionId ?? (_sessionId = NewId.NextId());
        }

        /// <summary>
        /// Gets the source IP address.
        /// </summary>
        /// <returns>Returns the source IP address.</returns>
        protected virtual string GetSourceIPAddress()
        {
            return "127.0.0.1";
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>Returns the current user.</returns>
        protected virtual ClaimsPrincipal GetUser()
        {
            return ClaimsPrincipal.Current;
        }

        private IMessage GetMessage(object message)
        {
            var current = message as IMessage;
            if (current != null)
            {
                return current;
            }
            return message == null ? new Message() : new Message(message);
        }

        private IMessage GetMessage(string message, EndPointMetaData endPoint)
        {
            if (message == null)
            {
                if (endPoint.RequestType != typeof(object))
                {
                    return new Message(JsonConvert.DeserializeObject("{}", endPoint.RequestType));
                }
                return new Message();
            }
            return new Message(JsonConvert.DeserializeObject(message, endPoint.RequestType));
        }

        private IMessage GetMessage(object message, EndPointMetaData endPoint)
        {
            if (message != null && message.GetType() == endPoint.RequestType)
            {
                return new Message(message);
            }
            if (message != null)
            {
                var content = JsonConvert.SerializeObject((message as EventMessage)?.Body ?? message);
                return new Message(JsonConvert.DeserializeObject(content, endPoint.RequestType));
            }
            return new Message(JsonConvert.DeserializeObject("{}", endPoint.RequestType));
        }
    }
}