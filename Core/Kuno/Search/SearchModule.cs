/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using System.Reflection;
using Autofac;
using Kuno.Reflection;
using Module = Autofac.Module;

namespace Kuno.Search
{
    /// <summary>
    /// Autofac module that registers search dependencies.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class SearchModule : Module
    {
        private readonly KunoStack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModule" /> class.
        /// </summary>
        /// <param name="stack">The current stack.</param>
        public SearchModule(KunoStack stack)
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

            builder.Register(c => new InMemorySearchContext())
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.Register(c => new SearchFacade(c.Resolve<IComponentContext>()))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            builder.RegisterGeneric(typeof(SearchIndex<>))
                .As(typeof(ISearchIndex<>))
                .PropertiesAutowired()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(_stack.Assemblies.ToArray())
                .Where(e => e.GetBaseAndContractTypes().Any(x => x == typeof(ISearchIndex<>)))
                .As(instance =>
                {
                    var interfaces = instance.GetInterfaces().Where(e => e.GetTypeInfo().IsGenericType && e.GetGenericTypeDefinition() == typeof(ISearchIndex<>));
                    return interfaces.Select(e => typeof(ISearchIndex<>).MakeGenericType(e.GetGenericArguments()[0]));
                })
                .PropertiesAutowired()
                .InstancePerDependency();
        }
    }
}