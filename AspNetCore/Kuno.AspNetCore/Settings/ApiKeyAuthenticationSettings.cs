/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.AspNetCore.Settings
{
    /// <summary>
    /// Options to enable API key authentication.
    /// </summary>
    public class ApiKeyAuthenticationSettings
    {
        /// <summary>
        /// Gets or sets a value indicating API authentication is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if API authentication is allowed; otherwise, <c>false</c>.
        /// </value>
        public bool Allow { get; set; } = false;

        /// <summary>
        /// Gets or sets the decryption key.
        /// </summary>
        /// <value>
        /// The decryption key.
        /// </value>
        public string Key { get; set; }
    }
}