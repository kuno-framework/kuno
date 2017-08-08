/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Kuno.Domain;
using Kuno.Reflection;

namespace Kuno.EntityFramework.Entities
{
    public class EntityContext : DbContext, IEntityContext
    {
        private readonly Assembly[] _assemblies;

        public EntityContext(EntityFrameworkOptions options) : base(options.Data.ConnectionString)
        {
                _assemblies = options.Assemblies.ToArray();

            // TODO: change to options
            this.Configuration.AutoDetectChangesEnabled = options.Data.AutoDetectChangesEnabled;
            this.Configuration.LazyLoadingEnabled = options.Data.LazyLoadingEnabled;
            this.Configuration.ProxyCreationEnabled = options.Data.ProxyCreationEnabled;
            this.Configuration.ValidateOnSaveEnabled = options.Data.ValidateOnSaveEnabled;
        }


        public Task Add<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot
        {
            this.Set<TEntity>().AddRange(instances);

            return this.SaveChangesAsync();
        }

        public Task Clear<TEntity>() where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();
            set.RemoveRange(set);

            return this.SaveChangesAsync();
        }

        public Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            return set.AnyAsync(expression);
        }

        public Task<bool> Exists<TEntity>(string id) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            return set.AnyAsync(e => e.Id == id);
        }

        public Task<TEntity> Find<TEntity>(string id) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            return set.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> Find<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            var result = await set.Where(expression).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TEntity>> Find<TEntity>() where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            var result = await set.ToListAsync();

            return result;
        }

        public Task Remove<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot
        {
            var ids = instances.Select(e => e.Id).ToList();

            var set = this.Set<TEntity>();
            set.RemoveRange(set.Where(e => ids.Contains(e.Id)));

            return this.SaveChangesAsync();
        }

        public Task Update<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot
        {
            this.Set<TEntity>().AddOrUpdate(instances);

            return this.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.SafelyGetTypes(typeof(IAggregateRoot)).Union(assembly.SafelyGetTypes(typeof(IEntity))))
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