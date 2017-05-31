/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// Describes the operations available on a single path. A Path Item may be empty, due to ACL constraints. The path itself is still exposed to the documentation viewer but they will not know which operations and parameters are available.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#pathItemObject"/>
    public class PathItem
    {
        /// <summary>
        /// Gets or sets the definition of a DELETE operation on this path.
        /// </summary>
        /// <value>
        /// The definition of a DELETE operation on this path.
        /// </value>
        public Operation Delete { get; set; }

        /// <summary>
        /// Gets the JSON extension data.
        /// </summary>
        /// <value>
        /// The JSON extension data.
        /// </value>
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the definition of a GET operation on this path.
        /// </summary>
        /// <value>
        /// The definition of a GET operation on this path.
        /// </value>
        public Operation Get { get; set; }

        /// <summary>
        /// Gets or sets the definition of a HEAD operation on this path.
        /// </summary>
        /// <value>
        /// The definition of a HEAD operation on this path.
        /// </value>
        public Operation Head { get; set; }

        /// <summary>
        /// Gets or sets the definition of a OPTIONS operation on this path.
        /// </summary>
        /// <value>
        /// The definition of a OPTIONS operation on this path.
        /// </value>
        public Operation Options { get; set; }

        /// <summary>
        /// Gets or sets the list of parameters that are applicable for all the operations described under this path. These parameters can be overridden at the operation level, but cannot be removed there. The list MUST NOT include duplicated parameters. A unique parameter is defined by a combination of a name and location. The list can use the Reference Object to link to parameters that are defined at the Swagger Object's parameters. There can be one "body" parameter at most.
        /// </summary>
        /// <value>
        /// The list of parameters that are applicable for all the operations described under this path.
        /// </value>
        public IList<IParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the definition of a PATCH operation on this path.
        /// </summary>
        /// <value>
        /// The definition of a PATCH operation on this path.
        /// </value>
        public Operation Patch { get; set; }

        /// <summary>
        /// Gets or sets the definition of a POST operation on this path.
        /// </summary>
        /// <value>
        /// The definition of a POST operation on this path.
        /// </value>
        public Operation Post { get; set; }

        /// <summary>
        /// Gets or sets the definition of a GET operation on this path.
        /// </summary>
        /// <value>
        /// The definition of a GET operation on this path.
        /// </value>
        public Operation Put { get; set; }

        /// <summary>
        /// Allows for an external definition of this path item. The referenced structure MUST be in the format of a Path Item Object. If there are conflicts between the referenced definition and this Path Item's definition, the behavior is undefined.
        /// </summary>
        /// <value>
        /// The external definition of this path item.
        /// </value>
        [JsonProperty("$ref")]
        public string Ref { get; set; }
    }
}