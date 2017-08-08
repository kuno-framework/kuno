/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Threading.Tasks;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Routes messages to remote endpoints.
    /// </summary>
    public interface IRemoteRouter
    {
        /// <summary>
        /// Determines whether this instance can route the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if this instance can route the specified request; otherwise, <c>false</c>.</returns>
        bool CanRoute(Request request);

        /// <summary>
        /// Routes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="parentContext">The parent context.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        Task<MessageResult> Route(Request request, ExecutionContext parentContext, TimeSpan? timeout = null);
    }
}