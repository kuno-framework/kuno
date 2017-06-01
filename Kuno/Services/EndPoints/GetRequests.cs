/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kuno.Services.Logging;
using Kuno.Validation;

namespace Kuno.Services.EndPoints
{
    /// <summary>
    /// Gets requests that have been executed in the application context.
    /// </summary>
    [EndPoint("_system/requests", Method = "GET")]
    public class GetRequests : EndPoint<GetRequestsRequest, IEnumerable<RequestEntry>>
    {
        private readonly IRequestLog _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequests" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public GetRequests(IRequestLog source)
        {
            Argument.NotNull(source, nameof(source));

            _source = source;
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns the response to the request.</returns>
        public override async Task<IEnumerable<RequestEntry>> ReceiveAsync(GetRequestsRequest instance)
        {
            var result = await _source.GetEntries(instance.Start, instance.End).ConfigureAwait(false);

            return result.Where(e => e.Path == null || !e.Path.StartsWith("_"));
        }
    }
}