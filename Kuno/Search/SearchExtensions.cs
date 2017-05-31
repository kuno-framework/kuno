/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kuno.Validation;

namespace Kuno.Search
{
    /// <summary>
    /// Contains extension methods for search.
    /// </summary>
    public static class SearchExtensions
    {
        /// <summary>
        /// Searches for the specified text and returns a query with the layered expression.
        /// </summary>
        /// <typeparam name="T">The instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="text">The search text.</param>
        /// <returns>Returns a query with the layered expression.</returns>
        public static IQueryable<T> Contains<T>(this IQueryable<T> instance, string text)
        {
            Argument.NotNull(instance, nameof(instance));
            if (string.IsNullOrWhiteSpace(text))
            {
                return instance;
            }

            var t = Expression.Parameter(typeof(T));
            Expression body = Expression.Constant(false);

            var containsMethod = typeof(string).GetMethod("Contains"
                , new[] {typeof(string)});

            var toLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);

            var toStringMethod = typeof(object).GetMethod("ToString");

            var stringProperties = typeof(T).GetProperties()
                .Where(property => property.PropertyType == typeof(string));

            foreach (var property in stringProperties)
            {
                var stringValue = Expression.Call(Expression.Property(t, property.Name),
                    toStringMethod);

                var updated = Expression.Call(stringValue, toLowerMethod);

                var nextExpression = Expression.Call(updated,
                    containsMethod,
                    Expression.Constant(text.ToLower()));

                body = Expression.OrElse(body, nextExpression);
            }

            return instance.Where(Expression.Lambda<Func<T, bool>>(body, t));
        }
    }
}