/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno.Services
{
    /// <summary>
    /// A service is a single unit of solution logic that represents a single business capability.
    /// This service type does not receive message data and does not return a value.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Gets or sets the current context.
        /// </summary>
        /// <value>The current context.</value>
        ExecutionContext Context { get; set; }

        /// <summary>
        /// Gets the current request.
        /// </summary>
        /// <value>The current request.</value>
        Request Request { get; }

        /// <summary>
        /// Called when the endpoint is created and started.
        /// </summary>
        void OnStart();
    }

    /// <summary>
    /// A service is a single unit of solution logic that represents a single business capability.  This service type takes in a message
    /// of the specified type and does not return a value.
    /// </summary>
    /// <typeparam name="TMessage">The type of message that this endpoint can receive.</typeparam>
    public interface IService<in TMessage> : IService
    {
        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        /// <param name="instance">The instance.</param>
        Task Receive(TMessage instance);
    }

    /// <summary>
    /// A service is a single unit of solution logic that represents a single business capability.  This service type takes in a message
    /// of the specified type and returns a value of the specified type.
    /// </summary>
    /// <typeparam name="TRequest">The type of message that this endpoint can receive.</typeparam>
    /// <typeparam name="TResponse">The type of message this endpoint returns.</typeparam>
    public interface IService<in TRequest, TResponse> : IService
    {
        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns the response to the request.</returns>
        Task<TResponse> Receive(TRequest instance);
    }
}