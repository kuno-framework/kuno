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
using Kuno.Services.Registry;
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
        public virtual async Task<MessageResult> Route(Request request, FunctionInfo function, ExecutionContext parentContext, TimeSpan? timeout = null)
        {
            CancellationTokenSource source;
            //if (timeout.HasValue || endPoint.Timeout.HasValue)
            //{
            //    source = new CancellationTokenSource(timeout ?? endPoint.Timeout.Value);
            //}
            //else
            //{
                source = new CancellationTokenSource();
            //}

            var context = new ExecutionContext(request, function, source.Token, parentContext);

            var handler = _components.Resolve(function.FunctionType);
            var service = handler as IFunction;
            if (service != null)
            {
                service.Context = context;
            }

            object body = request.Message.Body;
            var parameterType = function.ReceiveMethod.GetParameters().First().ParameterType;
            if (request.Message.Body == null || request.Message.Body.GetType() != parameterType)
            {
                body = JsonConvert.DeserializeObject(request.Message.Body, parameterType);
            }

            await ((Task)function.ReceiveMethod.Invoke(handler, new[] { body })).ConfigureAwait(false);

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