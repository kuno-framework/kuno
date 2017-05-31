/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kuno.Configuration;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Services.Logging
{
    internal class InMemoryRequestLog : IRequestLog
    {
        private readonly ApplicationInformation _environment;

        /// <summary>
        /// The lock for the instances.
        /// </summary>
        protected readonly ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The in-memory items.
        /// </summary>
        protected readonly List<RequestEntry> Instances = new List<RequestEntry>();

        public InMemoryRequestLog(ApplicationInformation environment)
        {
            _environment = environment;
        }

        public Task Append(Request entry)
        {
            Argument.NotNull(entry, nameof(entry));

            CacheLock.EnterWriteLock();
            try
            {
                Instances.Add(new RequestEntry(entry, _environment));
            }
            finally
            {
                CacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        public Task<IEnumerable<RequestEntry>> GetEntries(DateTimeOffset? start, DateTimeOffset? end)
        {
            CacheLock.EnterReadLock();
            try
            {
                start = start ?? DateTimeOffset.Now.LocalDateTime.AddDays(-1);
                end = end ?? DateTimeOffset.Now.LocalDateTime;
                return Task.FromResult(Instances.Where(e => e.TimeStamp >= start && e.TimeStamp <= end).AsEnumerable());
            }
            finally
            {
                CacheLock.ExitReadLock();
            }
        }
    }
}