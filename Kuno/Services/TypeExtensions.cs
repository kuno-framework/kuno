/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Kuno.Reflection;
using Kuno.Services.Inventory;
using Kuno.Validation;

namespace Kuno.Services
{
    /// <summary>
    /// Extensions for types within messaging.
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Assembly, XDocument> _commentsCache = new ConcurrentDictionary<Assembly, XDocument>();

        /// <summary>
        /// Gets the XML comments.
        /// </summary>
        /// <param name="assembly">The instance.</param>
        /// <returns>Returns the XML comments.</returns>
        public static XDocument GetComments(this Assembly assembly)
        {
            return _commentsCache.GetOrAdd(assembly, a =>
            {
                var path = Path.Combine(Path.GetDirectoryName(a.Location), Path.GetFileNameWithoutExtension(a.Location) + ".xml");
                if (File.Exists(path))
                {
                    return XDocument.Load(path);
                }
                return null;
            });
        }

        /// <summary>
        /// Gets the XML comments.
        /// </summary>
        /// <param name="type">The instance.</param>
        /// <returns>Returns the XML comments.</returns>
        public static Comments GetComments(this Type type)
        {
            var document = type.GetTypeInfo().Assembly.GetComments();
            if (document != null)
            {
                var node = document.XPathSelectElement("//member[@name=\"T:" + type.FullName + "\"]");
                if (node != null)
                {
                    return new Comments(node);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the XML comments.
        /// </summary>
        /// <param name="property">The instance.</param>
        /// <returns>Returns the XML comments.</returns>
        public static Comments GetComments(this PropertyInfo property)
        {
            var document = property.DeclaringType.GetTypeInfo().Assembly.GetComments();
            if (document != null)
            {
                var node = document.XPathSelectElement("//member[@name=\"P:" + property.DeclaringType.FullName + "." + property.Name + "\"]");
                if (node != null)
                {
                    return new Comments(node);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the path for the endPoint.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the path for the type.</returns>
        public static string GetPath(this Type type)
        {
            return type.GetAllAttributes<EndPointAttribute>().Select(e => e.Path).FirstOrDefault();
        }


        /// <summary>
        /// Gets any defined rules for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns any defined rules for the specified type.</returns>
        public static Type[] GetRules(this Type type)
        {
            return type.GetTypeInfo().Assembly.SafelyGetTypes(typeof(IValidate<>).MakeGenericType(type));
        }

        /// <summary>
        /// Gets the timeout for the endPoint.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the timeout for the type.</returns>
        public static TimeSpan? GetTimeout(this Type type)
        {
            var attribute = type.GetAllAttributes<EndPointAttribute>().FirstOrDefault();
            if (attribute != null && attribute.Timeout > 0)
            {
                return TimeSpan.FromMilliseconds(attribute.Timeout);
            }
            return null;
        }

        /// <summary>
        /// Gets the version for the endPoint.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the path for the type.</returns>
        public static int GetVersion(this Type type)
        {
            return type.GetAllAttributes<EndPointAttribute>().FirstOrDefault()?.Version ?? 1;
        }

        /// <summary>
        /// Determines whether the specified type is dynamic.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is dynamic; otherwise, <c>false</c>.</returns>
        public static bool IsDynamic(this Type type)
        {
            return typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type);
        }
    }
}