/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Validates an object instance based on security rules.
    /// </summary>
    /// <typeparam name="TValue">The type to validate.</typeparam>
    public interface ISecurityRule<in TValue> : IValidate<TValue>
    {
    }
}