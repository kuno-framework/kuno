/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Kuno.Configuration;
using Kuno.Domain;
using Kuno.Logging;
using Kuno.Reflection;
using Kuno.Search;
using Kuno.Services;
using Kuno.Services.Messaging;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;
#if core
using Microsoft.Extensions.DependencyModel;
#else
using System.IO;

#endif

namespace Kuno
{
    /// <summary>
    /// The host and main entry point to the stack.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class KunoStack : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KunoStack" /> class.
        /// </summary>
        /// <param name="markers">Item markers used to identify assemblies.</param>
        public KunoStack(params object[] markers)
        {
            this.Include(this.GetType());
            this.Include(markers);

            var builder = new ContainerBuilder();

            builder.RegisterModule(new ConfigurationModule(this));

            foreach (var module in this.Assemblies.SafelyGetTypes<Module>().Where(e => e.GetAllAttributes<AutoLoadAttribute>().Any()))
            {
                if (module.GetConstructors().SingleOrDefault()?.GetParameters().Length == 0)
                {
                    builder.RegisterModule((Module)Activator.CreateInstance(module));
                }
                if (module.GetConstructors().SingleOrDefault()?.GetParameters().SingleOrDefault()?.ParameterType == typeof(KunoStack))
                {
                    builder.RegisterModule((Module)Activator.CreateInstance(module, this));
                }
            }

            this.Container = builder.Build();
        }

        /// <summary>
        /// Gets the assemblies that are used for loading components.
        /// </summary>
        /// <value>The assemblies that are used for loading components.</value>
        public ObservableCollection<Assembly> Assemblies { get; private set; } = new ObservableCollection<Assembly>();

        /// <summary>
        /// Gets the configured <see cref="IConfiguration" />.
        /// </summary>
        /// <value>The configured <see cref="IConfiguration" />.</value>
        public IConfiguration Configuration => this.Container.Resolve<IConfiguration>();

        /// <summary>
        /// Gets the configured <see cref="IContainer" />.
        /// </summary>
        public IContainer Container { get; private set; }

        /// <summary>
        /// Gets the configured <see cref="IDomainFacade" />.
        /// </summary>
        /// <value>The configured <see cref="IDomainFacade" />.</value>
        public IDomainFacade Domain => this.Container.Resolve<IDomainFacade>();

        /// <summary>
        /// Gets the API information.
        /// </summary>
        /// <value>
        /// The API information.
        /// </value>
        public ApplicationInformation Information => this.Container.Resolve<ApplicationInformation>();

        /// <summary>
        /// Gets the configured <see cref="ILogger" />.
        /// </summary>
        /// <value>The configured <see cref="ILogger" />.</value>
        public ILogger Logger => this.Container.Resolve<ILogger>();

        /// <summary>
        /// Gets the configured <see cref="ISearchFacade" />.
        /// </summary>
        /// <value>The configured <see cref="ISearchFacade" />.</value>
        public ISearchFacade Search => this.Container.Resolve<ISearchFacade>();

#if !core
        private IEnumerable<Assembly> GetAssemblies()
        {
            var current = Assembly.GetEntryAssembly();
            if (current != null)
            {
                yield return current;
                foreach (var assembly in Directory.GetFiles(Path.GetDirectoryName(current.Location), current.GetName().Name.Split('.')[0] + "*.dll"))
                {
                    yield return Assembly.LoadFrom(assembly);
                }
            }
        }
#endif

#if core
        private IEnumerable<Assembly> GetAssemblies()
        {
            var dependencies = DependencyContext.Default;
            foreach (var compilationLibrary in dependencies.RuntimeLibraries)
            {
                if (DiscoveryService.Ignores.Any(e => compilationLibrary.Name.StartsWith(e)))
                {
                    continue;
                }

                var assemblyName = new AssemblyName(compilationLibrary.Name);

                yield return Assembly.Load(assemblyName);
            }
        }
#endif

        /// <summary>
        /// Includes or registers additional assemblies identified by the specified markers.
        /// </summary>
        /// <param name="markers">The markers.</param>
        public void Include(params object[] markers)
        {
            if (!markers?.Any() ?? true)
            {
                var list = new List<Assembly>(this.GetAssemblies());

                foreach (var source in list.Distinct().Except(this.Assemblies))
                {
                    this.Assemblies.Add(source);
                }
            }
            else
            {
                var current = markers.Select(e =>
                                     {
                                         var type = e as Type;
                                         if (type != null)
                                         {
                                             return type.GetTypeInfo().Assembly;
                                         }
                                         var assembly = e as Assembly;
                                         if (assembly != null)
                                         {
                                             return assembly;
                                         }
                                         return e.GetType().GetTypeInfo().Assembly;
                                     })
                                     .Distinct();
                foreach (var item in current)
                {
                    this.Assemblies.Add(item);
                }
            }
        }

        /// <summary>
        /// Publishes the specified events to any configured publish-subscribe endpoint.
        /// </summary>
        /// <param name="channel">The message channel.</param>
        /// <param name="message">The message to publish.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task Publish(string channel, object message = null)
        {
            return this.Container.Resolve<IMessageGateway>().Publish(channel, message);
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <param name="instance">The event instance.</param>
        public void Publish(Event instance)
        {
            this.Container.Resolve<IMessageGateway>().Publish(instance);
        }

        /// <summary>
        /// Sends the specified command to the configured point-to-point endPoint.
        /// </summary>
        /// <param name="message">The command to send.</param>
        /// <param name="timeout">The request timeout.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task<MessageResult> Send(object message, TimeSpan? timeout = null)
        {
            return this.Container.Resolve<IMessageGateway>().Send(message, timeout: timeout);
        }

        /// <summary>
        /// Sends the specified command to the configured point-to-point endPoint.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="timeout">The request timeout.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public async Task<MessageResult<T>> Send<T>(string path, TimeSpan? timeout = null)
        {
            var result = await this.Container.Resolve<IMessageGateway>().Send(path, timeout: timeout).ConfigureAwait(false);

            return new MessageResult<T>(result);
        }

        /// <summary>
        /// Sends the specified command to the configured point-to-point endPoint.
        /// </summary>
        /// <param name="message">The command to send.</param>
        /// <param name="timeout">The request timeout.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public async Task<MessageResult<T>> Send<T>(object message, TimeSpan? timeout = null)
        {
            var result = await this.Container.Resolve<IMessageGateway>().Send(message, timeout: timeout).ConfigureAwait(false);

            return new MessageResult<T>(result);
        }

        /// <summary>
        /// Sends the specified command to the configured point-to-point endPoint.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="message">The command to send.</param>
        /// <param name="timeout">The request timeout.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task<MessageResult> Send(string path, object message, TimeSpan? timeout = null)
        {
            return this.Container.Resolve<IMessageGateway>().Send(path, message, timeout: timeout);
        }

        /// <summary>
        /// Sends the an empty command to the configured point-to-point endPoint.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="timeout">The request timeout.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task<MessageResult> Send(string path, TimeSpan? timeout = null)
        {
            return this.Container.Resolve<IMessageGateway>().Send(path, null, timeout);
        }

        /// <summary>
        /// Sends the specified command to the configured point-to-point endPoint.
        /// </summary>
        /// <param name="path">The path to the receiver.</param>
        /// <param name="command">The serialized command to send.</param>
        /// <param name="timeout">The request timeout.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public Task<MessageResult> Send(string path, string command, TimeSpan? timeout = null)
        {
            return this.Container.Resolve<IMessageGateway>().Send(path, command, timeout: timeout);
        }

        #region IDisposable Implementation

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="KunoStack" /> class.
        /// </summary>
        ~KunoStack()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
                this.Container.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        #endregion
    }
}