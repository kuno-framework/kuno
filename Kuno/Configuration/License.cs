/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Configuration
{
    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#licenseObject"/>
    public class License
    {
        /// <summary>
        /// Gets or sets the license name used for the API.
        /// </summary>
        /// <value>
        /// The license name used for the API.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        /// <value>
        /// The URL to the license used for the API. MUST be in the format of a URL.
        /// </value>
        public string Url { get; set; }
    }
}