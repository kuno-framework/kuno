/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Kuno.Domain;
using Kuno.Reflection;
using Kuno.Search;
using Kuno.Services.Logging;
using Kuno.Services.Messaging;
using Kuno.Services.Pipeline;

namespace Kuno.Services
{
    /// <summary>
    /// A service is a single unit of solution logic that represents a single business capability.
    /// </summary>
    public abstract class BaseService : IService
    {
        /// <summary>
        /// Gets the configured <see cref="IDomainFacade" />.
        /// </summary>
        /// <value>The configured <see cref="IDomainFacade" />.</value>
        public IDomainFacade Domain => this.Components.Resolve<IDomainFacade>();

        /// <summary>
        /// Gets the configured <see cref="ISearchFacade" />.
        /// </summary>
        /// <value>The configured <see cref="ISearchFacade" />.</value>
        public ISearchFacade Search => this.Components.Resolve<ISearchFacade>();

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>The current context.</value>
        protected internal ExecutionContext Context => ((IService) this).Context;

        /// <summary>
        /// Gets the configured <see cref="IComponentContext" /> instance.
        /// </summary>
        /// <value>The configured <see cref="IComponentContext" /> instance.</value>
        internal IComponentContext Components { get; set; }

        ExecutionContext IService.Context { get; set; }

        /// <summary>
        /// Gets the current request.
        /// </summary>
        /// <value>The current request.</value>
        public Request Request => this.Context.Request;

        /// <summary>
        /// Called when the endpoint is created and started.
        /// </summary>
        public virtual void OnStart()
        {
        }

        /// <summary>
        /// Adds an event to be raised when the execution is successful.
        /// </summary>
        /// <param name="instance">The event to add.</param>
        public void AddRaisedEvent(Event instance)
        {
            this.Context.AddRaisedEvent(instance);
        }

        /// <summary>
        /// Sends the specified command to the configured point-to-point endpoint.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task<MessageResult> Send(object message)
        {
            var attribute = message.GetType().GetAllAttributes<RequestAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                return this.Components.Resolve<IMessageGateway>().Send(attribute.Path, message, this.Context);
            }
            return this.Components.Resolve<IMessageGateway>().Send(message, this.Context);
        }

        /// <summary>
        /// Sends the specified command to the configured point-to-point endpoint.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public async Task<MessageResult<T>> Send<T>(object message)
        {
            var result = await this.Send(message).ConfigureAwait(false);

            return new MessageResult<T>(result);
        }
    }

    /// <summary>
    /// A service is a single unit of solution logic that represents a single business capability.  This endpoint type does not receive message data and does
    /// not return a value.
    /// </summary>
    public abstract class Service : BaseService, IService<object>
    {
        async Task IService<object>.Receive(object instance)
        {
            await this.Components.Resolve<ValidateMessage>().Execute(this.Context).ConfigureAwait(false);

            if (!this.Context.ValidationErrors.Any())
            {
                try
                {
                    if (!this.Context.CancellationToken.IsCancellationRequested)
                    {
                        await this.ReceiveAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception exception)
                {
                    this.Context.SetException(exception);
                }
            }
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        public virtual void Receive()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        public virtual Task ReceiveAsync()
        {
            this.Receive();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Responds to the request with the specified message.
        /// </summary>
        /// <param name="instance">The message instance to respond with.</param>
        protected void Respond(object instance)
        {
            this.Context.Response = instance;
        }
    }

    /// <summary>
    /// A service is a single unit of solution logic that represents a single business capability.  This endpoint type takes in a message
    /// of the specified type and does not return a value.
    /// </summary>
    public abstract class Service<TMessage> : BaseService, IService<TMessage>
    {
        async Task IService<TMessage>.Receive(TMessage instance)
        {
            await this.Components.Resolve<ValidateMessage>().Execute(this.Context).ConfigureAwait(false);

            if (!this.Context.ValidationErrors.Any())
            {
                try
                {
                    if (!this.Context.CancellationToken.IsCancellationRequested)
                    {
                        await this.ReceiveAsync(instance).ConfigureAwait(false);
                    }
                }
                catch (Exception exception)
                {
                    this.Context.SetException(exception);
                }
            }
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        public virtual void Receive(TMessage instance)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        public virtual Task ReceiveAsync(TMessage instance)
        {
            this.Receive(instance);

            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// A service is a single unit of solution logic that represents a single business capability.  This endpoint type takes in a message
    /// of the specified type and returns a value of the specified type.
    /// </summary>
    public abstract class Service<TMessage, TResponse> : BaseService, IService<TMessage, TResponse>
    {
        async Task<TResponse> IService<TMessage, TResponse>.Receive(TMessage instance)
        {
            await this.Components.Resolve<ValidateMessage>().Execute(this.Context).ConfigureAwait(false);

            var result = default(TResponse);
            if (!this.Context.ValidationErrors.Any())
            {
                try
                {
                    if (!this.Context.CancellationToken.IsCancellationRequested)
                    {
                        result = await this.ReceiveAsync(instance).ConfigureAwait(false);

                        this.Context.Response = result;

                        if (result is Event)
                        {
                            this.AddRaisedEvent(result as Event);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.Context.SetException(exception);
                }
            }
            return result;
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns the response to the request.</returns>
        public virtual TResponse Receive(TMessage instance)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns the response to the request.</returns>
        public virtual Task<TResponse> ReceiveAsync(TMessage instance)
        {
            return Task.FromResult(this.Receive(instance));
        }
    }
}