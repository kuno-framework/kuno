/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Threading.Tasks;
using Kuno.Services.Registry;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Routes requests locally within the current process.
    /// </summary>
    public interface IRequestRouter
    {
        /// <summary>
        /// Routes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="endPoint">The endpoint to dispatch to.</param>
        /// <param name="parentContext">The parent context.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        Task<MessageResult> Route(Request request, FunctionInfo endPoint, ExecutionContext parentContext, TimeSpan? timeout = null);
    }
}