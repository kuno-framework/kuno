/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using System.Reflection;
using Akka.Actor;
using Autofac;
using Kuno.Akka.EndPoints;
using Kuno.Akka.Messaging;
using Kuno.Reflection;
using Module = Autofac.Module;

namespace Kuno.Akka.Modules
{
    /// <summary>
    /// Autofac module that configures the Akka.NET messaging block.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class AkkaModule : Module
    {
        private readonly KunoStack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="AkkaModule" /> class.
        /// </summary>
        /// <param name="stack">The current stack.</param>
        public AkkaModule(KunoStack stack)
        {
            _stack = stack;

            _stack.Include(this.GetType());
        }

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ServicesCoordinator>().AsSelf();

            builder.Register(c => new AkkaMessageDispatcher(c.Resolve<ActorSystem>(), c.Resolve<IComponentContext>()))
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(_stack.Assemblies.ToArray())
                .Where(e => e.GetBaseAndContractTypes().Any(x => x == typeof(ActorBase)))
                .AsSelf()
                .InstancePerDependency()
                .PropertiesAutowired();

            _stack.Assemblies.CollectionChanged += (sender, args) =>
            {
                _stack.Use(b =>
                {
                    b.RegisterAssemblyTypes(args.NewItems.OfType<Assembly>().ToArray())
                        .Where(e => e.GetBaseAndContractTypes().Any(x => x == typeof(ActorBase)))
                        .AsSelf()
                        .InstancePerDependency()
                        .PropertiesAutowired();
                });
            };
        }
    }
}