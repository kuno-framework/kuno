/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kuno.Reflection
{
    /// <summary>
    /// Contains extensions for <see cref="Type" /> classes.
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Assembly, Type[]> LoadedTypes = new ConcurrentDictionary<Assembly, Type[]>();

        private static readonly HashSet<Type> PrimitiveTypes = new HashSet<Type>
        {
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(string),
            typeof(char),
            typeof(decimal),
            typeof(int),
            typeof(uint),
            typeof(short),
            typeof(ushort),
            typeof(long),
            typeof(ulong),
            typeof(Guid)
        };

        /// <summary>
        /// Returns all custom attributes of the specified type.
        /// </summary>
        /// <typeparam name="T">The attribute type</typeparam>
        /// <param name="type">The instance.</param>
        /// <returns>Returns all custom attributes of the specified type.</returns>
        public static IEnumerable<T> GetAllAttributes<T>(this Type type) where T : Attribute
        {
            var target = new List<T>();
            do
            {
                target.AddRange(type.GetTypeInfo().GetCustomAttributes<T>());
                target.AddRange(type.GetInterfaces().SelectMany(e => e.GetAllAttributes<T>()));
                type = type.GetTypeInfo().BaseType;
            }
            while (type != null);

            return target.AsEnumerable();
        }

        /// <summary>
        /// Returns all custom attributes of the specified type.
        /// </summary>
        /// <typeparam name="T">The attribute type</typeparam>
        /// <param name="method">The instance.</param>
        /// <returns>Returns all custom attributes of the specified type.</returns>
        public static IEnumerable<T> GetAllAttributes<T>(this MethodInfo method) where T : Attribute
        {
            var target = new List<T>();
            target.AddRange(method.GetCustomAttributes<T>());
            return target.AsEnumerable();
        }

        /// <summary>
        /// Gets all base and contract types.
        /// </summary>
        /// <param name="type">The instance.</param>
        /// <returns>Returns all base and contract types.</returns>
        public static IEnumerable<Type> GetBaseAndContractTypes(this Type type)
        {
            return type.GetBaseTypes().Concat(type.GetInterfaces()).SelectMany(GetTypeAndGeneric).Where(t => t != type && t != typeof(object));
        }

        /// <summary>
        /// Gets all base types in the hierarchy.
        /// </summary>
        /// <param name="type">The instance.</param>
        /// <returns>Returns all base types in the hierarchy.</returns>
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            var currentType = type;
            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.GetTypeInfo().BaseType;
            }
        }

        /// <summary>
        /// Gets all properties recursively.
        /// </summary>
        /// <param name="type">The root type.</param>
        /// <returns>All discovered properties.</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesRecursive(this Type type)
        {
            var seenNames = new HashSet<string>();

            var currentTypeInfo = type.GetTypeInfo();

            while (currentTypeInfo.AsType() != typeof(object))
            {
                var unseenProperties = currentTypeInfo.DeclaredProperties.Where(p => p.CanRead &&
                                                                                     p.GetMethod.IsPublic &&
                                                                                     !p.GetMethod.IsStatic &&
                                                                                     (p.Name != "Item" || p.GetIndexParameters().Length == 0) &&
                                                                                     !seenNames.Contains(p.Name));

                foreach (var propertyInfo in unseenProperties)
                {
                    seenNames.Add(propertyInfo.Name);
                    yield return propertyInfo;
                }

                currentTypeInfo = currentTypeInfo.BaseType.GetTypeInfo();
            }
        }

        /// <summary>
        /// Determines whether the type is a dictionary.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if it is a dictionary; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDictionary(this Type instance)
        {
            return instance.GetInterfaces().Any(e => e.GetTypeInfo().IsGenericType && e.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        /// <summary>
        /// Determines whether the type is nullable.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(this Type instance)
        {
            return instance.GetTypeInfo().IsGenericType && instance.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether the type is primitive.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if primitive; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrimitive(this Type instance)
        {
            return instance.GetTypeInfo().IsPrimitive || PrimitiveTypes.Contains(instance);
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assemblies">The instance.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes<T>(this IEnumerable<Assembly> assemblies)
        {
            return SafelyGetTypes(assemblies, typeof(T));
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assemblies">The instance.</param>
        /// <param name="type">The parent type.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes(this IEnumerable<Assembly> assemblies, Type type)
        {
            if (type.GetTypeInfo().IsGenericTypeDefinition)
            {
                return assemblies.SelectMany(e => e.SafelyGetTypes()).Where(e => e.GetBaseAndContractTypes().Any(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == type)).ToArray();
            }
            return assemblies.SelectMany(e => e.SafelyGetTypes()).Where(e => e != null && type.IsAssignableFrom(e)).ToArray();
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assemblies">The instance.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(e => e.SafelyGetTypes()).ToArray();
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assembly">The instance.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes(this Assembly assembly)
        {
            return LoadedTypes.GetOrAdd(assembly, a =>
            {
                try
                {
                    return assembly.GetTypes().Where(e => e != null).ToArray();
                }
                catch (ReflectionTypeLoadException exception)
                {
                    return exception.Types.Where(x => x != null).ToArray();
                }
                catch
                {
                    return new Type[0];
                }
            });
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assembly">The instance.</param>
        /// <param name="type">The parent type.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes(this Assembly assembly, Type type)
        {
            return assembly.SafelyGetTypes().Where(e => e != null && type.IsAssignableFrom(e)).ToArray();
        }

        private static IEnumerable<Type> GetTypeAndGeneric(Type type)
        {
            yield return type;
            if (type.GetTypeInfo().IsGenericType && !type.GetTypeInfo().ContainsGenericParameters)
            {
                yield return type.GetGenericTypeDefinition();
            }
        }
    }
}