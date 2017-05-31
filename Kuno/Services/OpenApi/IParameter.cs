/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// Describes a single operation parameter.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#parameterObject"/>
    public interface IParameter
    {
        /// <summary>
        /// Gets or sets the brief description of the parameter. This could contain examples of use. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>
        /// The brief description of the parameter. This could contain examples of use. GFM syntax can be used for rich text representation.
        /// </value>
        string Description { get; set; }

        /// <summary>
        /// Gets the JSON extension data.
        /// </summary>
        /// <value>
        /// The JSON extension data.
        /// </value>
        Dictionary<string, object> Extensions { get; }

        /// <summary>
        /// Gets or sets the location of the parameter. Possible values are "query", "header", "path", "formData" or "body".
        /// </summary>
        /// <value>
        /// The location of the parameter. Possible values are "query", "header", "path", "formData" or "body".
        /// </value>
        string In { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter. Parameter names are case sensitive.
        /// </summary>
        /// <value>
        /// The name of the parameter. Parameter names are case sensitive.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// 	Determines whether this parameter is mandatory. If the parameter is in "path", this property is required and its value MUST be true. Otherwise, the property MAY be included and its default value is false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this parameter is mandatory; otherwise, <c>false</c>.
        /// </value>
        bool Required { get; set; }
    }
}