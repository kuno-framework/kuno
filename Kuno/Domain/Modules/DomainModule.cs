/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Autofac;
using Kuno.Caching;

namespace Kuno.Domain.Modules
{
    /// <summary>
    /// An Autofac module that wires up dependencies for the domain module.
    /// </summary>
    internal class DomainModule : Module
    {
        private readonly ApplicationStack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModule" /> class.
        /// </summary>
        /// <param name="stack">The stack.</param>
        public DomainModule(ApplicationStack stack)
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

            builder.Register(c => new DomainFacade(c.Resolve<IComponentContext>(), c.Resolve<ICacheManager>()))
                .As<IDomainFacade>()
                .SingleInstance();

            builder.Register(e => new InMemoryEntityContext())
                .As<IEntityContext>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .PropertiesAutowired()
                .SingleInstance();
        }
    }
}