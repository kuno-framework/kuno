/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#externalDocumentationObject"/>
    public class ExternalDocs
    {
        /// <summary>
        /// Gets or sets the short description of the target documentation. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>
        /// The short description of the target documentation. GFM syntax can be used for rich text representation.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL for the target documentation. Value MUST be in the format of a URL.
        /// </summary>
        /// <value>
        /// The URL for the target documentation. Value MUST be in the format of a URL.
        /// </value>
        public string Url { get; set; }
    }
}