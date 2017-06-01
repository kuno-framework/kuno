/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Kuno.Services.Inventory;
using Kuno.Services.Pipeline;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// A local <see cref="IRequestRouter" /> implementation.
    /// </summary>
    /// <seealso cref="IRequestRouter" />
    public class RequestRouter : IRequestRouter
    {
        private readonly IComponentContext _components;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestRouter" /> class.
        /// </summary>
        /// <param name="components">The configured <see cref="IComponentContext" />.</param>
        public RequestRouter(IComponentContext components)
        {
            _components = components;
        }

        /// <inheritdoc />
        public virtual async Task<MessageResult> Route(Request request, EndPointMetaData endPoint, ExecutionContext parentContext, TimeSpan? timeout = null)
        {
            CancellationTokenSource source;
            if (timeout.HasValue || endPoint.Timeout.HasValue)
            {
                source = new CancellationTokenSource(timeout ?? endPoint.Timeout.Value);
            }
            else
            {
                source = new CancellationTokenSource();
            }

            var context = new ExecutionContext(request, endPoint, source.Token, parentContext);

            var handler = _components.Resolve(endPoint.EndPointType);
            var service = handler as IEndPoint;
            if (service != null)
            {
                service.Context = context;
            }

            var body = request.Message.Body;
            var parameterType = endPoint.InvokeMethod.GetParameters().First().ParameterType;
            if (body == null || body.GetType() != parameterType)
            {
                body = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body ?? ""), parameterType);
            }

            await ((Task)endPoint.InvokeMethod.Invoke(handler, new[] { body })).ConfigureAwait(false);

            await this.Complete(context).ConfigureAwait(false);

            return new MessageResult(context);
        }

        /// <summary>
        /// Runs steps to complete the current execution context.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        protected virtual async Task Complete(ExecutionContext context)
        {
            var steps = new List<IMessageExecutionStep>
            {
                _components.Resolve<HandleException>(),
                _components.Resolve<Complete>(),
                _components.Resolve<PublishEvents>(),
                _components.Resolve<LogCompletion>()
            };
            foreach (var step in steps)
            {
                await step.Execute(context).ConfigureAwait(false);
            }
        }
    }
}