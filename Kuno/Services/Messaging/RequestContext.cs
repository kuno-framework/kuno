/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using Kuno.Reflection;
using Kuno.Services.Registry;
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
        public Request Resolve(object message, EndPoint endPoint, Request parent = null)
        {
            return new Request
            {
                CorrelationId = parent?.CorrelationId ?? this.GetCorrelationId(),
                SourceAddress = parent?.SourceAddress ?? this.GetSourceIPAddress(),
                SessionId = parent?.SessionId ?? this.GetSession(),
                User = parent?.User ?? this.GetUser(),
                Parent = parent,
                Path = endPoint.Path,
                Message = this.GetMessage(message, endPoint.Function)
            };
        }

        /// <inheritdoc />
        public Request Resolve(object message, Subscription subscription, Request parent = null)
        {
            return new Request
            {
                CorrelationId = parent?.CorrelationId ?? this.GetCorrelationId(),
                SourceAddress = parent?.SourceAddress ?? this.GetSourceIPAddress(),
                SessionId = parent?.SessionId ?? this.GetSession(),
                User = parent?.User ?? this.GetUser(),
                Parent = parent,
                Channel = subscription.Channel,
                Message = this.GetMessage(message, subscription.Function)
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
        public Request Resolve(string path, EndPoint endPoint, Request parent = null)
        {
            return new Request
            {
                CorrelationId = parent?.CorrelationId ?? this.GetCorrelationId(),
                SourceAddress = parent?.SourceAddress ?? this.GetSourceIPAddress(),
                SessionId = parent?.SessionId ?? this.GetSession(),
                User = parent?.User ?? this.GetUser(),
                Parent = parent,
                Path = endPoint.Path,
                Message = this.GetMessage(path, endPoint)
            };
        }

        /// <inheritdoc />
        public Request Resolve(EventMessage instance, Request parent)
        {
            return new Request
            {
                CorrelationId = parent?.CorrelationId,
                SourceAddress = parent?.SourceAddress,
                SessionId = parent?.SessionId,
                User = parent?.User,
                Parent = parent,
                Message = instance,
                Channel = instance.Name
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

        private IMessage GetMessage(string message, EndPoint endPoint)
        {
            if (message == null)
            {
                if (endPoint.Function.RequestType != typeof(object))
                {
                    return new Message(JsonConvert.DeserializeObject("{}", endPoint.Function.RequestType));
                }
                return new Message();
            }
            return new Message(JsonConvert.DeserializeObject(message, endPoint.Function.RequestType));
        }

        private IMessage GetMessage(object message, FunctionInfo function)
        {
            if (message is IMessage)
            {
                return message as IMessage;
            }
            if (message != null && message.GetType() == function.RequestType)
            {
                return new Message(message);
            }
            if (message != null)
            {
                string content;
                if (message is string)
                {
                    content = message as string;
                }
                else
                {
                    content = JsonConvert.SerializeObject((message as EventMessage)?.Body ?? message);
                }
                return new Message(JsonConvert.DeserializeObject(content, function.RequestType));
            }
            return new Message(JsonConvert.DeserializeObject("{}", function.RequestType));
        }
    }
}