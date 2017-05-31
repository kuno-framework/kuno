/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;

namespace Kuno.Validation
{
    /// <summary>
    /// Defines a contract for a self-validating class.
    /// </summary>
    public interface IValidate
    {
        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>
        /// Returns validation errors that were found as a result of the validation.
        /// An empty collection means that no errors were found.
        /// </returns>
        IEnumerable<ValidationError> Validate();
    }
}