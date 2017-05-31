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
using Slalom.Stacks.Services.Inventory;
using Slalom.Stacks.Services.Logging;
using Slalom.Stacks.Validation;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Provides a context for the flow of execution for a request.  This context stays with any subsequent requests.
    /// </summary>
    public class ExecutionContext
    {
        private readonly List<EventMessage> _raisedEvents = new List<EventMessage>();
        private readonly List<ValidationError> _validationErrors = new List<ValidationError>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContext" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="endPoint">The current endpoint.</param>
        /// <param name="cancellationToken">The cancellation.</param>
        /// <param name="parent">The parent.</param>
        public ExecutionContext(Request request, EndPointMetaData endPoint, CancellationToken cancellationToken, ExecutionContext parent = null)
        {
            this.Request = request;
            this.EndPoint = endPoint;
            this.Parent = parent;
            this.CancellationToken = cancellationToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContext" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        public ExecutionContext(Request request, ExecutionContext context)
        {
            this.Request = request;
            this.Parent = context?.Parent;
            this.CancellationToken = context?.CancellationToken ?? CancellationToken.None;
        }

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        /// <value>The cancellation token.</value>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets the date and time that execution completed.
        /// </summary>
        /// <value>The date and time that execution completed.</value>
        public DateTimeOffset? Completed { get; private set; }

        /// <summary>
        /// Gets the registry entry.
        /// </summary>
        /// <value>The registry entry.</value>
        public EndPointMetaData EndPoint { get; }

        /// <summary>
        /// Gets the raised exception, if any.
        /// </summary>
        /// <value>The raised exception, if any.</value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the execution was successful.
        /// </summary>
        /// <value><c>true</c> if execution was successful; otherwise, <c>false</c>.</value>
        public bool IsSuccessful => !this.ValidationErrors.Any() && this.Exception == null;

        /// <summary>
        /// Gets the parent context.
        /// </summary>
        /// <value>The parent context.</value>
        public ExecutionContext Parent { get; }

        /// <summary>
        /// Gets any raised events.
        /// </summary>
        /// <value>The raised events.</value>
        public IEnumerable<EventMessage> RaisedEvents => _raisedEvents.AsEnumerable();

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public Request Request { get; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public object Response { get; set; }

        /// <summary>
        /// Gets the date and time that execution started.
        /// </summary>
        /// <value>The date and time that execution started.</value>
        public DateTimeOffset Started { get; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets any validation errors that were raised as part of execution.
        /// </summary>
        /// <value>The validation errors that were raised as part of execution.</value>
        public IEnumerable<ValidationError> ValidationErrors => _validationErrors.AsEnumerable();

        /// <summary>
        /// Adds the raised event.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void AddRaisedEvent(Event instance)
        {
            _raisedEvents.Add(new EventMessage(this.Request.Message.Id, instance));
        }

        /// <summary>
        /// Adds the validation errors.
        /// </summary>
        /// <param name="errors">The erros to add.</param>
        public void AddValidationErrors(IEnumerable<ValidationError> errors)
        {
            _validationErrors.AddRange(errors);
        }

        /// <summary>
        /// Completes the context.
        /// </summary>
        public void Complete()
        {
            this.Completed = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Sets the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void SetException(Exception exception)
        {
            this.Exception = exception;
        }
    }
}