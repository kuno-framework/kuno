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
using Kuno.Configuration;
using Kuno.Logging;
using Kuno.Services.Logging;
using Kuno.Services.Messaging;

namespace Kuno.Services.Pipeline
{
    /// <summary>
    /// The log completion step of the endpoint execution pipeline.
    /// </summary>
    /// <seealso cref="Kuno.Services.Pipeline.IMessageExecutionStep" />
    internal class LogCompletion : IMessageExecutionStep
    {
        private readonly IResponseLog _actions;
        private readonly ApplicationInformation _environmentContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCompletion" /> class.
        /// </summary>
        /// <param name="components">The component context.</param>
        public LogCompletion(IComponentContext components)
        {
            _actions = components.Resolve<IResponseLog>();
            _logger = components.Resolve<ILogger>();
            _environmentContext = components.Resolve<ApplicationInformation>();
        }

        /// <inheritdoc />
        public Task Execute(ExecutionContext context)
        {
            if (context.Request.Path?.StartsWith("_") == true && context.IsSuccessful)
            {
                return Task.FromResult(0);
            }

            var tasks = new List<Task> { _actions.Append(new ResponseEntry(context, _environmentContext)) };

            var name = context.Request.Path ?? context.Request.Message.Name;
            if (!context.IsSuccessful)
            {
                if (context.Exception != null)
                {
                    _logger.Error(context.Exception, "An unhandled exception was raised while executing \"" + name + "\".", context);
                }
                else if (context.ValidationErrors?.Any() ?? false)
                {
                    _logger.Error("Execution completed with validation errors while executing \"" + name + "\": " + string.Join("; ", context.ValidationErrors.Select(e => e.Type + ": " + e.Message)), context);
                }
                else
                {
                    _logger.Error("Execution completed unsuccessfully while executing \"" + name + "\".", context);
                }
            }
            else
            {
                _logger.Verbose("Successfully executed \"" + name + "\".", context);
            }

            return Task.WhenAll(tasks);
        }
    }
}