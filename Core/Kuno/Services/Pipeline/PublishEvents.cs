/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Kuno.Services.Logging;
using Kuno.Services.Messaging;

namespace Kuno.Services.Pipeline
{
    /// <summary>
    /// The publish events step of the endpoint execution pipeline.
    /// </summary>
    /// <seealso cref="Kuno.Services.Pipeline.IMessageExecutionStep" />
    internal class PublishEvents : IMessageExecutionStep
    {
        private readonly IEventStore _eventStore;
        private readonly IMessageGateway _messageGateway;
        private TaskRunner _tasks;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishEvents" /> class.
        /// </summary>
        /// <param name="components">The current component context.</param>
        public PublishEvents(IComponentContext components)
        {
            _messageGateway = components.Resolve<IMessageGateway>();
            _eventStore = components.Resolve<IEventStore>();
            _tasks = components.Resolve<TaskRunner>();
        }

        /// <inheritdoc />
        public async Task Execute(ExecutionContext context)
        {
            if (context.IsSuccessful)
            {
                var raisedEvents = context.RaisedEvents.Union(new[] { context.Response as EventMessage }).Where(e => e != null).ToArray();
                foreach (var instance in raisedEvents)
                {
                    await _eventStore.Append(instance).ConfigureAwait(false);

#pragma warning disable 4014
                    _tasks.Add(() => Task.Run(() => _messageGateway.Publish(instance, context)));
#pragma warning restore 4014
                }
            }
        }
    }
}