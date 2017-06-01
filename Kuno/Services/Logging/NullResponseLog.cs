/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kuno.Services.Logging
{
    internal class NullResponseLog : IResponseLog
    {
        public Task Append(ResponseEntry entry)
        {
            return Task.FromResult(0);
        }

        public Task<IEnumerable<ResponseEntry>> GetEntries(DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            throw new NotImplementedException();
        }
    }
}