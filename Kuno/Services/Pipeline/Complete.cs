/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno.Services.Pipeline
{
    /// <summary>
    /// The completion step of the usecase execution pipeline.
    /// </summary>
    /// <seealso cref="Kuno.Services.Pipeline.IMessageExecutionStep" />
    internal class Complete : IMessageExecutionStep
    {
        /// <inheritdoc />
        public Task Execute(ExecutionContext context)
        {
            context.Complete();

            return Task.FromResult(0);
        }
    }
}