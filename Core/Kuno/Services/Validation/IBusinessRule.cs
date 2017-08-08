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
    /// Validates an object instance using a business rule.
    /// </summary>
    /// <typeparam name="TValue">The type of message to validate.</typeparam>
    public interface IBusinessRule<in TValue> : IValidate<TValue>
    {
    }
}