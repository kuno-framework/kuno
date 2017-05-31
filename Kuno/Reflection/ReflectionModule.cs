/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Autofac;
using Kuno.Logging;

namespace Kuno.Reflection
{
    /// <summary>
    /// Autofac module that registers search dependencies.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class ReflectionModule : Module
    {
        private readonly Stack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionModule" /> class.
        /// </summary>
        /// <param name="stack">The stack.</param>
        public ReflectionModule(Stack stack)
        {
            _stack = stack;
        }

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

            builder.Register(c => new DiscoveryService(c.Resolve<ILogger>())).AsSelf().AsImplementedInterfaces();
        }
    }
}