/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno.Services.Pipeline
{
    /// <summary>
    /// A defined step of the endpoint execution pipeline.
    /// </summary>
    internal interface IMessageExecutionStep
    {
        /// <summary>
        /// Executes the step of the endpoint execution pipeline.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task Execute(ExecutionContext context);
    }
}