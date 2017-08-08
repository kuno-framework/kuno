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
    /// Describes a single response from an API Operation.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#responseObject"/>
    public class Response
    {
        /// <summary>
        /// Gets or sets the short description of the response. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>
        /// The short description of the response. GFM syntax can be used for rich text representation.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the example of the response message.
        /// </summary>
        /// <value>
        /// The example of the response message.
        /// </value>
        public object Examples { get; set; }

        /// <summary>
        /// Gets the JSON extension data.
        /// </summary>
        /// <value>
        /// The JSON extension data.
        /// </value>
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the list of headers that are sent with the response.
        /// </summary>
        /// <value>
        /// The list of headers that are sent with the response.
        /// </value>
        public IDictionary<string, Header> Headers { get; set; }

        /// <summary>
        /// Gets or sets the definition of the response structure. It can be a primitive, an array or an object. If this field does not exist, it means no content is returned as part of the response. As an extension to the Schema Object, its root type value may also be "file". This SHOULD be accompanied by a relevant produces mime-type.
        /// </summary>
        /// <value>
        /// The definition of the response structure.
        /// </value>
        public Schema Schema { get; set; }
    }
}