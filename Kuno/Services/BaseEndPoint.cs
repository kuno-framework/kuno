using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno.Services
{
    /// <summary>
    /// An endpoint is a single unit of solution logic that can be accessed in-process or out-of-process.
    /// </summary>
    public abstract class BaseEndPoint : IEndPoint
    {
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
            var result = await this.Send(message);

            return new MessageResult<T>(result);
        }
    }
}
