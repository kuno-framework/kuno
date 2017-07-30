/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// Contains information about a function.
    /// </summary>
    public class FunctionInfo
    {
        /// <summary>
        /// Gets or sets the receive method.
        /// </summary>
        /// <value>
        /// The receive method.
        /// </value>
        public MethodInfo ReceiveMethod { get; set; }

        /// <summary>
        /// Gets or sets the function type.
        /// </summary>
        /// <value>
        /// The function type.
        /// </value>
        public Type FunctionType { get; set; }

        /// <summary>
        /// Gets or sets the endpoint summary.
        /// </summary>
        /// <value>The endpoint summary.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the request type.
        /// </summary>
        /// <value>The request type.</value>
        public Type RequestType { get; set; }

        /// <summary>
        /// Gets or sets the response type.
        /// </summary>
        /// <value>The response type.</value>
        public Type ResponseType { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        /// <value>The rules.</value>
        public List<FunctionRule> Rules { get; set; }

        /// <summary>
        /// Gets the function information from the specified type.
        /// </summary>
        /// <param name="function">The type of function.</param>
        /// <returns>Returns the function information from the specified type</returns>
        public static IEnumerable<FunctionInfo> Create(Type function)
        {
            var interfaces = function.GetInterfaces().Where(e => e.GetTypeInfo().IsGenericType && (e.GetGenericTypeDefinition() == typeof(IFunction<>) || e.GetGenericTypeDefinition() == typeof(IFunction<,>))).ToList();
            if (interfaces.Any())
            {
                var summary = function.GetComments();

                foreach (var item in interfaces)
                {
                    var method = item.GetMethod("Receive");
                    if (method.DeclaringType != null)
                    {
                        var requestType = method.GetParameters().FirstOrDefault()?.ParameterType;

                        yield return new FunctionInfo
                        {
                            RequestType = requestType,
                            ResponseType = GetResponseType(method),
                            Rules = requestType?.GetRules().Select(e => new FunctionRule(e)).ToList(),
                            ReceiveMethod = method,
                            Summary = summary?.Summary,
                            FunctionType = function
                        };
                    }
                }
            }
        }

        private static Type GetResponseType(MethodInfo method)
        {
            if (method.ReturnType == typeof(Task))
            {
                return null;
            }
            if (method?.ReturnType?.GetTypeInfo().IsGenericType == true && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return method.ReturnType.GetGenericArguments()[0];
            }
            return method.ReturnType;
        }
    }
}