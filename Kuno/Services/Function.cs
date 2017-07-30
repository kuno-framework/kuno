using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Kuno.Services.Pipeline;

namespace Kuno.Services
{
    /// <summary>
    /// A function is a single unit of solution logic that represents a single business capability.  This endpoint type does not receive message data and does
    /// not return a value.
    /// </summary>
    public abstract class Function : BaseFunction, IFunction<object>
    {
        async Task IFunction<object>.Receive(object instance)
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
    /// A function is a single unit of solution logic that represents a single business capability.  This endpoint type takes in a message
    /// of the specified type and does not return a value.
    /// </summary>
    public abstract class Function<TMessage> : BaseFunction, IFunction<TMessage>
    {
        async Task IFunction<TMessage>.Receive(TMessage instance)
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
    /// A function is a single unit of solution logic that represents a single business capability.  This endpoint type takes in a message
    /// of the specified type and returns a value of the specified type.
    /// </summary>
    public abstract class Function<TMessage, TResponse> : BaseFunction, IFunction<TMessage, TResponse>
    {
        async Task<TResponse> IFunction<TMessage, TResponse>.Receive(TMessage instance)
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