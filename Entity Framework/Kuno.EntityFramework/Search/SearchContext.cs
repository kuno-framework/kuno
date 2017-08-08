/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inflector;
using Kuno.Reflection;
using Kuno.Search;
using Kuno.Validation;

namespace Kuno.EntityFramework.Search
{
    /// <summary>
    /// An Entity Framework <see cref="ISearchContext" /> implementation.
    /// </summary>
    /// <seealso cref="ISearchContext" />
    internal class SearchContext : DbContext, ISearchContext
    {
        private readonly EntityFrameworkOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SearchContext(EntityFrameworkOptions options) : base(options.Search.ConnectionString)
        {
            Argument.NotNull(options, nameof(options));

            _options = options;
        }

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        public async Task AddAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            var table = CreateDataTable(instances);

            using (var connection = new SqlConnection(_options.Search.ConnectionString))
            {
                connection.Open();

                using (var copy = new SqlBulkCopy(connection))
                {
                    copy.DestinationTableName = string.Format(typeof(TSearchResult).Name.Pluralize());
                    foreach (var column in table.Columns)
                    {
                        var columnName = ((DataColumn) column).ColumnName;
                        var mapping = new SqlBulkCopyColumnMapping(columnName, columnName);
                        copy.ColumnMappings.Add(mapping);
                    }

                    await copy.WriteToServerAsync(table).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Clears all instances of the specified type.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <returns>A task for asynchronous programming.</returns>
        public async Task ClearAsync<TSearchResult>() where TSearchResult : class, ISearchResult
        {
            using (var connection = new SqlConnection(_options.Search.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("DELETE FROM [" + typeof(TSearchResult).Name.Pluralize() + "]", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task<TSearchResult> FindAsync<TSearchResult>(int id) where TSearchResult : class, ISearchResult
        {
            return this.Set<TSearchResult>().FindAsync(id);
        }

        /// <summary>
        /// Removes the instances that match the specified predicate.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to remove.</typeparam>
        /// <param name="predicate">The predicate used to filter.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task RemoveAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate) where TSearchResult : class, ISearchResult
        {
            this.Set<TSearchResult>().RemoveRange(this.Set<TSearchResult>().Where(predicate));

            return this.SaveChangesAsync();
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task RemoveAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            this.Set<TSearchResult>().RemoveRange(instances);

            return this.SaveChangesAsync();
        }

        /// <summary>
        /// Opens a query that can be used to filter and project.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of the instance.</typeparam>
        /// <param name="text">The text to use for search.</param>
        /// <returns>An IQueryable&lt;TAggregateRoot&gt; that can be used to filter and project.</returns>
        public IQueryable<TSearchResult> Search<TSearchResult>(string text = null) where TSearchResult : class, ISearchResult
        {
            return this.Set<TSearchResult>().AsNoTracking();
        }

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        public Task UpdateAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class, ISearchResult
        {
            this.Set<TSearchResult>().AddOrUpdate(instances);

            return this.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the specified instances using the specified predicate and update expression.
        /// </summary>
        /// <typeparam name="TSearchResult">The type of instance.</typeparam>
        /// <param name="predicate">The predicate used to filter.</param>
        /// <param name="expression">The expression used to update.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public Task UpdateAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate, Expression<Func<Type, Type>> expression) where TSearchResult : class, ISearchResult
        {
            throw new NotSupportedException();
        }

        public static DataTable CreateDataTable<T>(params T[] items) where T : ISearchResult
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in items)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }


        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.
        /// </param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in _options.Assemblies)
            {
                foreach (var type in assembly.SafelyGetTypes(typeof(ISearchResult)))
                {
                    if (type.IsAbstract || type.IsInterface)
                    {
                        continue;
                    }

                    entityMethod.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] { });
                }
            }
        }
    }
}