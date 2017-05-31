/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Validation
{
    /// <summary>
    /// Indicates the validation error type.
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// Indicates a type was not specified.
        /// </summary>
        None,

        /// <summary>
        /// Indicates an input validation message.
        /// </summary>
        Input,

        /// <summary>
        /// Indicates an security validation message.
        /// </summary>
        Security,

        /// <summary>
        /// Indicates a business validation message.
        /// </summary>
        Business
    }
}