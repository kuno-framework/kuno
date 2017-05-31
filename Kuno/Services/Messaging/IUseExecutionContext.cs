/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Contract for a class that expects execution context.
    /// </summary>
    public interface IUseExecutionContext
    {
        /// <summary>
        /// Sets the execution context to use.
        /// </summary>
        /// <param name="context">The execution context to use.</param>
        void UseContext(ExecutionContext context);
    }
}