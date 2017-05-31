/* 
 * Copyright (c) Stacks Contributors
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
    /// Gets responses that have been executed in the application context.
    /// </summary>
    [EndPoint("_system/responses", Method = "GET")]
    public class GetResponses : EndPoint<GetResponsesRequest, IEnumerable<ResponseEntry>>
    {
        private readonly IResponseLog _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetResponses" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public GetResponses(IResponseLog source)
        {
            Argument.NotNull(source, nameof(source));

            _source = source;
        }

        /// <summary>
        /// Receives the call to the endpoint.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns the response to the request.</returns>
        public override async Task<IEnumerable<ResponseEntry>> ReceiveAsync(GetResponsesRequest instance)
        {
            var result = await _source.GetEntries(instance.Start, instance.End);

            return result.Where(e => e.Path == null || !e.Path.StartsWith("_") || e.ValidationErrors != null);
        }
    }
}