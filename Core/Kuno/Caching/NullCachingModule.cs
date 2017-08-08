/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Autofac;

namespace Kuno.Caching
{
    /// <summary>
    /// An Autofac module to configure the caching dependencies.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class NullCachingModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">
        /// The builder through which components can be
        /// registered.
        /// </param>
        /// <remarks>Note that the ContainerBuilder parameter is unique to this module.</remarks>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new NullCacheManager())
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c => new NullCacheConnector())
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}