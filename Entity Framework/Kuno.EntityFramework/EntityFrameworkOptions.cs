/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Reflection;

namespace Kuno.EntityFramework
{
    /// <summary>
    /// Settings for Entity Framework search.
    /// </summary>
    public class SearchSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; } = "Data Source=.;Initial Catalog=Kuno.Search;Integrated Security=True;MultipleActiveResultSets=True";

        /// <summary>
        /// Gets or sets a value that indicates whether or not EF code first migrations should apply.
        /// </summary>
        /// <value>A value that indicates whether or not EF code first migrations should apply.</value>
        public bool EnableMigrations { get; set; } = false;

        /// <summary>
        /// Sets the connection string to use.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>Returns this instance for method chaining.</returns>
        public SearchSettings WithConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;

            return this;
        }
    }

    /// <summary>
    /// Settings for Entity Framework data.
    /// </summary>
    public class EntitySettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="M:System.Data.Entity.Infrastructure.DbChangeTracker.DetectChanges" />
        /// method is called automatically by methods of <see cref="T:System.Data.Entity.DbContext" /> and related classes.
        /// The default value is true.
        /// </summary>
        /// <value>
        /// <c>true</c> if should be called automatically; otherwise, <c>false</c>.
        /// </value>
        public bool AutoDetectChangesEnabled { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; } = "Data Source=.;Initial Catalog=Kuno;Integrated Security=True;MultipleActiveResultSets=True";

        /// <summary>
        /// Gets or sets a value that indicates whether or not EF code first migrations should apply.
        /// </summary>
        /// <value>A value that indicates whether or not EF code first migrations should apply.</value>
        public bool EnableMigrations { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether lazy loading of relationships exposed as
        /// navigation properties is enabled.  Lazy loading is disabled by default.
        /// </summary>
        /// <value>
        /// <c>true</c> if lazy loading is enabled; otherwise, <c>false</c> .
        /// </value>
        public bool LazyLoadingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the framework will create instances of
        /// dynamically generated proxy classes whenever it creates an instance of an entity type.
        /// Note that even if proxy creation is enabled with this flag, proxy instances will only
        /// be created for entity types that meet the requirements for being proxied.
        /// Proxy creation is disabled by default.
        /// </summary>
        /// <value>
        /// <c>true</c> if proxy creation is enabled; otherwise, <c>false</c> .
        /// </value>
        public bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tracked entities should be validated automatically when
        /// <see cref="M:System.Data.Entity.DbContext.SaveChanges" /> is invoked.
        /// The default value is false.
        /// </summary>
        public bool ValidateOnSaveEnabled { get; set; }

        /// <summary>
        /// Sets the connection string to use.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>Returns this instance for method chaining.</returns>
        public EntitySettings WithConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;

            return this;
        }
    }

    /// <summary>
    /// Options for Entity Framework.
    /// </summary>
    public class EntityFrameworkOptions
    {
        /// <summary>
        /// Gets or sets the data settings.
        /// </summary>
        /// <value>The data settings.</value>
        public EntitySettings Data { get; set; } = new EntitySettings();

        /// <summary>
        /// Gets or sets the search settings.
        /// </summary>
        /// <value>The search settings.</value>
        public SearchSettings Search { get; set; } = new SearchSettings();

        internal IEnumerable<Assembly> Assemblies { get; set; }
    }
}