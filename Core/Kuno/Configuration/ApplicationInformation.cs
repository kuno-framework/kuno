/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Newtonsoft.Json;

namespace Kuno.Configuration
{
    /// <summary>
    /// Contains information about the application or API that is used in external items like logs and discovery documents.
    /// </summary>
    public class ApplicationInformation
    {
        /// <summary>
        /// Gets or sets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        [JsonProperty("x-build")]
        public string Build { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the exposed API.
        /// </summary>
        /// <value>
        /// The contact information for the exposed API.
        /// </value>
        public Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the short description of the application. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>
        /// The short description of the application. GFM syntax can be used for rich text representation.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the environment name.
        /// </summary>
        /// <value>
        /// The environment name.
        /// </value>
        [JsonProperty("x-environment")]
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the license information for the exposed API.
        /// </summary>
        /// <value>
        /// The license information for the exposed API.
        /// </value>
        public License License { get; set; }

        /// <summary>
        /// Gets or sets the Terms of Service for the API.
        /// </summary>
        /// <value>
        /// The Terms of Service for the API.
        /// </value>
        public string TermsOfService { get; set; }

        /// <summary>
        /// Gets or sets the title of the application.
        /// </summary>
        /// <value>
        /// The title of the application.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version of the application API (not to be confused with the specification version).
        /// </summary>
        /// <value>
        /// The version of the application API (not to be confused with the specification version).
        /// </value>
        public string Version { get; set; }
    }
}