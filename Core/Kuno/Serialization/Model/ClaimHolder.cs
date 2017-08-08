/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Security.Claims;

namespace Kuno.Serialization.Model
{
    /// <summary>
    /// EndPointType used to hold a <see cref="Claim" />  for serialization.
    /// </summary>
    public class ClaimHolder
    {
        /// <summary>
        /// Gets or sets the claim type.
        /// </summary>
        /// <value>The claim type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        /// <value>The claim value.</value>
        public string Value { get; set; }
    }
}