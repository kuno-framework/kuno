/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// Describes a single operation parameter that is in the body.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#bodyObject"/>
    public class BodyParameter : IParameter
    {
        /// <summary>
        /// Gets or sets the schema defining the type used for the body parameter..
        /// </summary>
        /// <value>
        /// The schema defining the type used for the body parameter..
        /// </value>
        public Schema Schema { get; set; }

        /// <inheritdoc />
        public string Name { get; set; } = "body";

        /// <inheritdoc />
        public string In { get; set; } = "body";

        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        public bool Required { get; set; } = true;

        /// <inheritdoc />
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; } = new Dictionary<string, object>();
    }
}