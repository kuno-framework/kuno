/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Kuno.Caching;
using Kuno.Domain.Modules;
using Kuno.Logging;
using Kuno.Reflection;
using Kuno.Search;
using Kuno.Services.Modules;

namespace Kuno.Configuration
{
    /// <summary>
    /// An Autofac module that wires up root dependencies for the stack.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class ConfigurationModule : Module
    {
        private readonly Stack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationModule" /> class.
        /// </summary>
        /// <param name="stack">The current stack.</param>
        public ConfigurationModule(Stack stack)
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

            builder.Register(c =>
                   {
                       var currentDirectory = Directory.GetCurrentDirectory();
                       var configurationBuilder = new ConfigurationBuilder();
                       configurationBuilder.SetBasePath(currentDirectory);
                       configurationBuilder.AddJsonFile("appsettings.json", true, true);
                       configurationBuilder.AddJsonFile("stacks.json", true, true);
                       foreach (var path in Directory.GetFiles(currentDirectory, "stacks.*.json"))
                       {
                           configurationBuilder.AddJsonFile(Path.GetFileName(path), true, true);
                       }
                       if (Directory.Exists(Path.Combine(currentDirectory, "config")))
                       {
                           foreach (var path in Directory.GetFiles(currentDirectory, "config\\stacks**.json"))
                           {
                               configurationBuilder.AddJsonFile("config\\" + Path.GetFileName(path), true, true);
                           }
                       }
                       configurationBuilder.AddEnvironmentVariables();
                       return configurationBuilder.Build();
                   })
                   .As<IConfiguration>()
                   .SingleInstance();

            builder.RegisterType<ApplicationInformation>()
                   .SingleInstance()
                   .OnActivated(c =>
                   {
                       var configuration = c.Context.Resolve<IConfiguration>();
                       configuration.GetSection("Stacks")?.Bind(c.Instance);
                       configuration.GetReloadToken().RegisterChangeCallback(_ =>
                       {
                           configuration.GetSection("Stacks")?.Bind(c.Instance);
                       }, configuration);
                   });

            builder.RegisterModule(new DomainModule(_stack));
            builder.RegisterModule(new ServicesModule(_stack));
            builder.RegisterModule(new SearchModule(_stack));
            builder.RegisterModule(new ReflectionModule(_stack));

            builder.RegisterModule(new LoggingModule());
            builder.RegisterModule(new NullCachingModule());

            builder.RegisterType<DiscoveryService>()
                   .As<IDiscoverTypes>()
                   .SingleInstance();
        }
    }
}