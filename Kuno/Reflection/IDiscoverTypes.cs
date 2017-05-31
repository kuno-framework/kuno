/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;

namespace Kuno.Reflection
{
    /// <summary>
    /// Discovers types that exist in a given requestContext.
    /// </summary>
    public interface IDiscoverTypes
    {
        /// <summary>
        /// Finds all types in the requestContext.
        /// </summary>
        /// <typeparam name="TType">The type's base class or interface.</typeparam>
        /// <returns>Returns all types in the requestContext.</returns>
        IEnumerable<Type> Find<TType>();

        /// <summary>
        /// Finds available types that are assignable to the specified type.
        /// </summary>
        /// <returns>All available types that are assignable to the specified type.</returns>
        IEnumerable<Type> Find(Type type);
    }
}