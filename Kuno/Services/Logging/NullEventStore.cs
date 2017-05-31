/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno.Services.Logging
{
    internal class NullEventStore : IEventStore
    {
        public Task<IEnumerable<EventEntry>> GetEvents(DateTimeOffset? start, DateTimeOffset? end)
        {
            return Task.FromResult(new EventEntry[0].AsEnumerable());
        }

        public Task Append(EventMessage instance)
        {
            return Task.FromResult(0);
        }
    }
}