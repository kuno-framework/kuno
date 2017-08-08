/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Data.Entity;
using Autofac;

namespace Kuno.EntityFramework.Entities
{
    public class EntityFrameworkEntitiesModule : Module
    {
        private readonly EntityFrameworkOptions _options;
        private readonly KunoStack _stack;

        public EntityFrameworkEntitiesModule(KunoStack stack, EntityFrameworkOptions options)
        {
            _stack = stack;
            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new EntityContext(_options))
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            if (_options.Data.EnableMigrations)
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EntityContext>());
            }
        }
    }
}