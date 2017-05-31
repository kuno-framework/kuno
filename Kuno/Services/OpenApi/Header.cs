/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// Describes a single header from an API Operation
    /// </summary>
    /// <seealso cref="PartialSchema" />
    /// <seealso href="http://swagger.io/specification/#headerObject"/>
    public class Header : PartialSchema
    {
        /// <summary>
        /// Gets or sets the short description of the header.
        /// </summary>
        /// <value>
        /// The short description of the header.
        /// </value>
        public string Description { get; set; }
    }
}