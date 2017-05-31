/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Validates and object instance using input rules.
    /// </summary>
    /// <typeparam name="TValue">The type to validate.</typeparam>
    public interface IInputRule<in TValue> : IValidate<TValue>
    {
    }
}