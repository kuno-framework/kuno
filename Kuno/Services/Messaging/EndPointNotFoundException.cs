/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Exception that is raised when no endpoint can be found for an incoming request.
    /// </summary>
    /// <seealso cref="System.InvalidOperationException" />
    public class EndPointNotFoundException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndPointNotFoundException" /> class.
        /// </summary>
        /// <param name="request">The current request.</param>
        public EndPointNotFoundException(Request request) : base($"An endpoint could not be found for the path \"{request.Path}\".")
        {
        }
    }
}