/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Autofac;
using Kuno.Validation;

namespace Kuno.Caching
{
    /// <summary>
    /// Contains configuration extensions for caching blocks.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Configures the container to use a local cache.
        /// </summary>
        /// <param name="container">The current container.</param>
        public static void UseLocalCache(this ApplicationStack container)
        {
            Argument.NotNull(container, nameof(container));

            container.Container.Update(builder => { builder.RegisterModule(new LocalCacheModule()); });
        }
    }
}