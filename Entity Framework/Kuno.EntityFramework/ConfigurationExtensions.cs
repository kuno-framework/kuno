/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Autofac;
using Kuno.EntityFramework.Entities;
using Kuno.EntityFramework.Search;
using Kuno.Validation;
using Microsoft.Extensions.Configuration;

namespace Kuno.EntityFramework
{
    /// <summary>
    /// Contains extension methods to add Entity Framework.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds Entity Framework.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static KunoStack UseEntityFramework(this KunoStack instance, Action<EntitySettings> configuration = null)
        {
            Argument.NotNull(instance, nameof(instance));

            var options = new EntityFrameworkOptions();
            configuration?.Invoke(options.Data);
            instance.Configuration.GetSection("Kuno:EntityFramework").Bind(options);
            options.Assemblies = instance.Assemblies;

            instance.Include(typeof(EntityContext));

            instance.Use(e => { e.RegisterModule(new EntityFrameworkEntitiesModule(instance, options)); });
            return instance;
        }

        /// <summary>
        /// Adds Entity Framework Search.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static KunoStack UseEntityFrameworkSearch(this KunoStack instance, Action<SearchSettings> configuration = null)
        {
            Argument.NotNull(instance, nameof(instance));

            var options = new EntityFrameworkOptions();
            configuration?.Invoke(options.Search);
            instance.Configuration.GetSection("Kuno:EntityFramework").Bind(options);
            options.Assemblies = instance.Assemblies;

            instance.Include(typeof(SearchContext));

            instance.Use(e => { e.RegisterModule(new EntityFrameworkSearchModule(instance, options)); });

            return instance;
        }
    }
}