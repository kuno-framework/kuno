/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Threading.Tasks;
using Kuno.Logging;
using Kuno.Services.Logging;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Services.Pipeline
{
    /// <summary>
    /// The log startup step of the EndPoint execution pipeline.
    /// </summary>
    /// <seealso cref="Kuno.Services.Pipeline.IMessageExecutionStep" />
    internal class LogStart : IMessageExecutionStep
    {
        private readonly ILogger _logger;
        private readonly IRequestLog _requests;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogStart" /> class.
        /// </summary>
        /// <param name="logger">The configured logger.</param>
        /// <param name="requests">The configured request log.</param>
        public LogStart(ILogger logger, IRequestLog requests)
        {
            Argument.NotNull(logger, nameof(logger));

            _logger = logger;
            _requests = requests;
        }

        /// <inheritdoc />
        public async Task Execute(ExecutionContext context)
        {
            await _requests.Append(context.Request).ConfigureAwait(false);

            var message = context.Request.Message;
            if (message.Body != null && context.Request.Path != null)
            {
                _logger.Verbose("Executing \"" + message.Name + "\" at path \"" + context.Request.Path + "\".");
            }
            else if (message.Body != null)
            {
                _logger.Verbose("Executing \"" + message.Name + ".");
            }
            else
            {
                _logger.Verbose("Executing message at path \"" + context.Request.Path + "\".");
            }
        }
    }
}