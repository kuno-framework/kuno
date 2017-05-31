/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using Kuno.Reflection;
using Kuno.Validation;

#pragma warning disable 618

namespace Kuno
{
    /// <summary>
    /// Extensions for <see cref="Stack" /> instances.
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Autowires properties of registered instances.
        /// </summary>
        /// <typeparam name="TLimit">The type of the t limit.</typeparam>
        /// <typeparam name="TActivatorData">The type of the t activator data.</typeparam>
        /// <typeparam name="TRegistrationStyle">The type of the t registration style.</typeparam>
        /// <param name="registration">The registration.</param>
        /// <returns>IRegistrationBuilder&lt;TLimit, TActivatorData, TRegistrationStyle&gt;.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> AllPropertiesAutowired<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
            return registration.OnActivated(args => InjectProperties(args.Context, args.Instance, true));
        }

        /// <summary>
        /// Specifies how a type from a scanned assembly is mapped to a service.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TScanningActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to set service mapping on.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AsBaseAndContractTypes<TLimit, TScanningActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration) where TScanningActivatorData : ScanningActivatorData
        {
            return registration.As(instance => instance.GetBaseAndContractTypes());
        }

        /// <summary>
        /// Injects values into properties of the specified instance.
        /// </summary>
        /// <param name="context">The context to get the value from.</param>
        /// <param name="instance">The instance where properties should be set.</param>
        /// <param name="overrideSetValues">if set to <c>true</c> [override set values].</param>
        public static void InjectProperties(IComponentContext context, object instance, bool overrideSetValues)
        {
            Argument.NotNull(context, nameof(context));
            Argument.NotNull(instance, nameof(instance));

            foreach (var propertyInfo in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var propertyType = propertyInfo.PropertyType;

                if ((!propertyType.GetTypeInfo().IsValueType || propertyType.GetTypeInfo().IsEnum) && propertyInfo.GetIndexParameters().Length == 0 && context.IsRegistered(propertyType))
                {
                    var accessors = propertyInfo.GetAccessors(true);
                    if ((accessors.Length != 1 ||
                         !(accessors[0].ReturnType != typeof(void))) &&
                        (overrideSetValues || accessors.Length != 2 ||
                         propertyInfo.GetValue(instance, null) == null))
                    {
                        var obj = context.Resolve(propertyType);
                        propertyInfo.SetValue(instance, obj, null);
                    }
                }
            }
        }

        /// <summary>
        /// Resolves all instance of the specified type from the container.
        /// </summary>
        /// <returns>The resolved instances.</returns>
        /// <exception>Thrown when the <paramref name="instance" /> argument is null.</exception>
        public static IEnumerable<T> ResolveAll<T>(this IComponentContext instance)
        {
            Argument.NotNull(instance, nameof(instance));

            return instance.Resolve<IEnumerable<T>>();
        }

        /// <summary>
        /// Resolves all instance of the specified type from the container.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="type">The type to resolve.</param>
        /// <returns>The resolved instances.</returns>
        /// <exception>Thrown when the <paramref name="instance" /> argument is null.</exception>
        /// <exception>Thrown when the <paramref name="type" /> argument is null.</exception>
        public static IEnumerable<object> ResolveAll(this IComponentContext instance, Type type)
        {
            Argument.NotNull(instance, nameof(instance));
            Argument.NotNull(type, nameof(type));

            var target = ((IEnumerable<object>) instance.Resolve(typeof(IEnumerable<>).MakeGenericType(type))).ToList();

            foreach (var item in target)
            {
                instance.InjectProperties(item);
            }

            return target;
        }

        /// <summary>
        /// Updates the container with the specified builder configuration.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>The current instance for method chaining.</returns>
        /// <exception>Thrown when the <paramref name="instance" /> argument is null.</exception>
        /// <exception>Thrown when the <paramref name="configuration" /> argument is null.</exception>
        public static IContainer Update(this IContainer instance, Action<ContainerBuilder> configuration)
        {
            Argument.NotNull(instance, nameof(instance));
            Argument.NotNull(configuration, nameof(configuration));

            var builder = new ContainerBuilder();
            configuration.Invoke(builder);
            builder.Update(instance.ComponentRegistry);
            return instance;
        }

        /// <summary>
        /// Tells the stack to use the specified builder configuration.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>The current instance for method chaining.</returns>
        /// <exception>Thrown when the <paramref name="instance" /> argument is null.</exception>
        /// <exception>Thrown when the <paramref name="configuration" /> argument is null.</exception>
        public static Stack Use(this Stack instance, Action<ContainerBuilder> configuration)
        {
            Argument.NotNull(instance, nameof(instance));
            Argument.NotNull(configuration, nameof(configuration));

            var builder = new ContainerBuilder();
            configuration.Invoke(builder);
            builder.Update(instance.Container.ComponentRegistry);
            return instance;
        }
    }
}