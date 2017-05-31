/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Kuno.Services.Inventory;
using Kuno.Services.Logging;
using Kuno.Services.Messaging;
using Kuno.Services.Pipeline;
using Kuno.Services.Validation;
using Kuno.Validation;
using Module = Autofac.Module;

namespace Kuno.Services.Modules
{
    /// <summary>
    /// An Autofac module to configure the services dependencies.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class ServicesModule : Module
    {
        private readonly Stack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicesModule" /> class.
        /// </summary>
        public ServicesModule(Stack stack)
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

            builder.Register(c => new MessageGateway(c.Resolve<IComponentContext>()))
                .As<IMessageGateway>()
                .SingleInstance();

            builder.RegisterType<RequestRouter>().As<IRequestRouter>();
            builder.RegisterType<RemoteServiceInventory>().AsSelf().SingleInstance();

            builder.RegisterType<InMemoryEventStore>().As<IEventStore>().SingleInstance();

            builder.RegisterAssemblyTypes(_stack.Assemblies.Union(new[] { typeof(IMessageExecutionStep).GetTypeInfo().Assembly }).Distinct().ToArray())
                .Where(e => e.GetInterfaces().Any(x => x == typeof(IMessageExecutionStep)))
                .AsSelf();

            builder.RegisterType<ServiceInventory>()
                .AsSelf()
                .SingleInstance()
                .OnActivated(e =>
                {
                    e.Instance.Load(_stack.Assemblies.ToArray());
                });

            builder.RegisterType<RequestContext>()
                .As<IRequestContext>();

            builder.RegisterType<InMemoryRequestLog>().As<IRequestLog>().SingleInstance();
            builder.RegisterType<InMemoryResponseLog>().As<IResponseLog>().SingleInstance();
            builder.RegisterType<InMemoryEventStore>().As<IEventStore>().SingleInstance();

            builder.RegisterGeneric(typeof(MessageValidator<>));

            this.RegisterAssemblyTypes(builder, _stack.Assemblies.ToArray());

            _stack.Assemblies.CollectionChanged += this.HandleCollectionChanged;
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _stack.Use(builder => { this.RegisterAssemblyTypes(builder, e.NewItems.OfType<Assembly>().ToArray()); });
        }

        private void RegisterAssemblyTypes(ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(e => e.GetInterfaces().Any(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidate<>)))
                .AsBaseAndContractTypes()
                .AllPropertiesAutowired();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(e => e.GetInterfaces().Any(x => x == typeof(IEndPoint)))
                .AsBaseAndContractTypes()
                .AsSelf()
                .AllPropertiesAutowired()
                .OnActivated(e => { ((IEndPoint)e.Instance).OnStart(); });

            builder.RegisterAssemblyTypes(assemblies)
                .Where(e => e.GetInterfaces().Contains(typeof(IEventPublisher)))
                .As<IEventPublisher>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(e => e.GetInterfaces().Contains(typeof(IRemoteRouter)))
                .As<IRemoteRouter>()
                .AsSelf()
                .SingleInstance();
        }
    }
}