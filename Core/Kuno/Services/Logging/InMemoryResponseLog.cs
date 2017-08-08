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
using Kuno.Validation;

namespace Kuno.Services.Logging
{
    internal class InMemoryResponseLog : IResponseLog
    {
        /// <summary>
        /// The lock for the instances.
        /// </summary>
        protected readonly ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The in-memory items.
        /// </summary>
        protected readonly List<ResponseEntry> Instances = new List<ResponseEntry>();


        public Task Append(ResponseEntry entry)
        {
            Argument.NotNull(entry, nameof(entry));

            CacheLock.EnterWriteLock();
            try
            {
                Instances.Add(entry);
            }
            finally
            {
                CacheLock.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        public Task<IEnumerable<ResponseEntry>> GetEntries(DateTimeOffset? start = null, DateTimeOffset? end = null)
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