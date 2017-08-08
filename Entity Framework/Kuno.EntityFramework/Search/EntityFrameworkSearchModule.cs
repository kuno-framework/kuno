/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Data.Entity;
using Autofac;
using Kuno.Search;
using Kuno.Validation;

namespace Kuno.EntityFramework.Search
{
    /// <summary>
    /// An Autofac module for configuring the Entity Framework Search module.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class EntityFrameworkSearchModule : Module
    {
        private readonly EntityFrameworkOptions _options;
        private readonly KunoStack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSearchModule" /> class.
        /// </summary>
        /// <param name="stack">The configured stack.</param>
        /// <param name="options">The options to use.</param>
        public EntityFrameworkSearchModule(KunoStack stack, EntityFrameworkOptions options)
        {
            Argument.NotNull(options, nameof(options));

            _stack = stack;
            _options = options;
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

            builder.Register(c => new SearchContext(_options))
                   .AsSelf()
                   .As<ISearchContext>()
                   .AllPropertiesAutowired();

            if (_options.Search.EnableMigrations)
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SearchContext>());
            }
        }
    }
}