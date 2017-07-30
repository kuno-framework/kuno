/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.Services.Registry;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Resolves a request context using the specified parameters.
    /// </summary>
    public interface IRequestContext
    {
        /// <summary>
        /// Resolves the current request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="endPoint">The endpoint.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>The current request.</returns>
        Request Resolve(object message, FunctionInfo endPoint, Request parent = null);

        /// <summary>
        /// Resolves the current request.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="endPoint">The endpoint.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>The current request.</returns>
        Request Resolve(string path, FunctionInfo endPoint, Request parent = null);

        /// <summary>
        /// Resolves the current request.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>The current request.</returns>
        Request Resolve(EventMessage instance, Request parent);

        /// <summary>
        /// Resolves the current request.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="message">The message.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>The current request.</returns>
        Request Resolve(string path, object message, Request parent = null);
    }
}