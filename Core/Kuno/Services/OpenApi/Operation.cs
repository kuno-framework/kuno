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
    /// Describes a single API operation on a path.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#operationObject"/>
    public class Operation
    {
        /// <summary>
        /// Gets or sets the list of MIME types the operation can consume. This overrides the consumes definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        /// <value>
        /// The list of MIME types the operation can consume.
        /// </value>
        public IList<string> Consumes { get; set; }

        /// <summary>
        /// Declares this operation to be deprecated. Usage of the declared operation should be refrained. Default value is false.
        /// </summary>
        /// <value>
        /// A value indicating whether this instance is deprecated.
        /// </value>
        public bool? Deprecated { get; set; }

        /// <summary>
        /// Gets or sets the verbose explanation of the operation behavior. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>
        /// The verbose explanation of the operation behavior. GFM syntax can be used for rich text representation.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the JSON extension data.
        /// </summary>
        /// <value>
        /// The JSON extension data.
        /// </value>
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the additional external documentation for this operation.
        /// </summary>
        /// <value>
        /// The additional external documentation for this operation.
        /// </value>
        public ExternalDocs ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets the unique string used to identify the operation. The id MUST be unique among all operations described in the API. Tools and libraries MAY use the operationId to uniquely identify an operation, therefore, it is recommended to follow common programming naming conventions.
        /// </summary>
        /// <value>
        /// The unique string used to identify the operation.
        /// </value>
        public string OperationId { get; set; }

        /// <summary>
        /// Gets or sets the list of parameters that are applicable for this operation. If a parameter is already defined at the Path Item, the new definition will override it, but can never remove it. The list MUST NOT include duplicated parameters. A unique parameter is defined by a combination of a name and location. The list can use the Reference Object to link to parameters that are defined at the Swagger Object's parameters. There can be one "body" parameter at most.
        /// </summary>
        /// <value>
        /// The list of parameters that are applicable for this operation.
        /// </value>
        public IList<IParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the list of MIME types the operation can produce. This overrides the produces definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        /// <value>
        /// The list of MIME types the operation can produce. 
        /// </value>
        public IList<string> Produces { get; set; }

        /// <summary>
        /// Gets or sets the list of possible responses as they are returned from executing this operation.
        /// </summary>
        /// <value>
        /// The list of possible responses as they are returned from executing this operation.
        /// </value>
        public IDictionary<string, Response> Responses { get; set; }

        /// <summary>
        /// Gets or sets the transfer protocol for the operation. Values MUST be from the list: "http", "https", "ws", "wss". The value overrides the Swagger Object schemes definition.
        /// </summary>
        /// <value>
        /// The transfer protocol for the operation. Values MUST be from the list: "http", "https", "ws", "wss". The value overrides the Swagger Object schemes definition.
        /// </value>
        public IList<string> Schemes { get; set; }

        /// <summary>
        /// Gets or sets the declaration of which security schemes are applied for this operation. The list of values describes alternative security schemes that can be used (that is, there is a logical OR between the security requirements). This definition overrides any declared top-level security. To remove a top-level security declaration, an empty array can be used.
        /// </summary>
        /// <value>
        /// The declaration of which security schemes are applied for this operation.
        /// </value>
        public List<Dictionary<string, List<string>>> Security { get; set; }

        /// <summary>
        /// Gets or sets the short summary of what the operation does. For maximum readability in the swagger-ui, this field SHOULD be less than 120 characters.
        /// </summary>
        /// <value>
        /// The short summary of what the operation does. For maximum readability in the swagger-ui, this field SHOULD be less than 120 characters.
        /// </value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier.
        /// </summary>
        /// <value>
        /// The list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier.
        /// </value>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Adds a security requirement to the operation.
        /// </summary>
        /// <param name="definition">The security definition.</param>
        /// <param name="scopes">The security scopes.</param>
        public void IncludeSecurity(string definition, params string[] scopes)
        {
            if (this.Security == null)
            {
                this.Security = new List<Dictionary<string, List<string>>>();
            }
            var requirement = new Dictionary<string, List<string>>
            {
                { definition, new List<string>(scopes) }
            };
            this.Security.Add(requirement);
        }
    }
}