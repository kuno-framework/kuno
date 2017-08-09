using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Kuno.Domain;
using Kuno.Reflection;
using Kuno.Search;
using Kuno.Services.Messaging;

namespace Kuno.Services
{
    /// <summary>
    /// A function is a single unit of solution logic that represents a single business capability.
    /// </summary>
    public abstract class BaseFunction : IFunction
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
        internal ExecutionContext Context => ((IFunction) this).Context;

        /// <summary>
        /// Gets the configured <see cref="IComponentContext" /> instance.
        /// </summary>
        /// <value>The configured <see cref="IComponentContext" /> instance.</value>
        internal IComponentContext Components { get; set; }

        ExecutionContext IFunction.Context { get; set; }

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
}