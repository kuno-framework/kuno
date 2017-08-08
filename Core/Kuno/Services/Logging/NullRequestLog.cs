/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno.Services.Logging
{
    internal class NullRequestLog : IRequestLog
    {
        public Task Append(Request entry)
        {
            return Task.FromResult(0);
        }

        public Task<IEnumerable<RequestEntry>> GetEntries(DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            throw new NotImplementedException();
        }
    }
}