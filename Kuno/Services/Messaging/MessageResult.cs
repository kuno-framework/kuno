/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kuno.Serialization;
using Kuno.Validation;
using Newtonsoft.Json;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// The result of message execution.  Contains information about the execution and the response from
    /// the actor.
    /// </summary>
    public class MessageResult<T> : MessageResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResult" /> class.
        /// </summary>
        /// <param name="context">The completed context.</param>
        public MessageResult(ExecutionContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResult{T}" /> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public MessageResult(MessageResult instance)
        {
            this.CorrelationId = instance.CorrelationId;
            this.Started = instance.Started;
            this.Completed = instance.Completed;
            this.RaisedException = instance.RaisedException;
            this.ValidationErrors = instance.ValidationErrors.ToList();
            this.RequestId = instance.RequestId;
            this.IsCancelled = instance.IsCancelled;
            if (instance.Response is T)
            {
                this.Response = (T)instance.Response;
            }
            else if (instance.Response is string)
            {
                this.Response = JsonConvert.DeserializeObject<T>((string)instance.Response);
            }
            else
            {
                this.Response = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(instance.Response, DefaultSerializationSettings.Instance), DefaultSerializationSettings.Instance);
            }
        }

        /// <summary>
        /// Gets the message response.
        /// </summary>
        /// <value>The message response.</value>
        public new T Response
        {
            get => (T)base.Response;
            set => base.Response = value;
        }
    }

    /// <summary>
    /// The result of message execution.  Contains information about the execution and the response from
    /// the actor.
    /// </summary>
    public class MessageResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResult" /> class.
        /// </summary>
        protected MessageResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResult" /> class.
        /// </summary>
        /// <param name="context">The completed context.</param>
        public MessageResult(ExecutionContext context)
        {
            this.CorrelationId = context.Request.CorrelationId;
            this.Started = DateTimeOffset.UtcNow;
            this.Completed = context.Completed;
            this.RaisedException = context.Exception;
            this.Response = context.Response;
            this.ValidationErrors = context.ValidationErrors.ToList();
            this.RequestId = context.Request.Message.Id;
            this.IsCancelled = context.CancellationToken.IsCancellationRequested;
        }

        /// <summary>
        /// Gets the date and time completed.
        /// </summary>
        /// <value>The date and time completed.</value>
        public DateTimeOffset? Completed { get; protected set; }

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        /// <value>The correlation identifier.</value>
        public string CorrelationId { get; protected set; }

        /// <summary>
        /// Gets the time elapsed.
        /// </summary>
        /// <value>The time elapsed.</value>
        public TimeSpan? Elapsed
        {
            get
            {
                var timeSpan = this.Completed - this.Started;
                if (timeSpan != null)
                {
                    return new TimeSpan(Math.Max(timeSpan.Value.Ticks, 0));
                }
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this message execution was cancelled.
        /// </summary>
        /// <value><c>true</c> if this message execution was cancelled; otherwise, <c>false</c>.</value>
        public bool IsCancelled { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the execution was successful.
        /// </summary>
        /// <value><c>true</c> if the execution was successful; otherwise, <c>false</c>.</value>
        public bool IsSuccessful => !this.ValidationErrors.Any() && this.RaisedException == null;

        /// <summary>
        /// Gets the raised exception if any.
        /// </summary>
        /// <value>The raised exception.</value>
        public Exception RaisedException { get; protected set; }

        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public string RequestId { get; protected set; }

        /// <summary>
        /// Gets the message response.
        /// </summary>
        public object Response { get; internal set; }

        /// <summary>
        /// Gets or sets the date and time started.
        /// </summary>
        /// <value>The date and time started.</value>
        public DateTimeOffset Started { get; protected set; }

        /// <summary>
        /// Gets any validation errors that were raised.
        /// </summary>
        /// <value>The validation errors that were raised.</value>
        public IReadOnlyList<ValidationError> ValidationErrors { get; protected set; }
    }
}