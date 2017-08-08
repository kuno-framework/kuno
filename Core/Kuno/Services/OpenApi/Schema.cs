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
    /// The Schema Object allows the definition of input and output data types. These types can be objects, but also primitives and arrays. This object is based on the JSON Schema Specification Draft 4 and uses a predefined subset of it. On top of this subset, there are extensions provided by this specification to allow for more complete documentation.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#schemaObject"/>
    public class Schema
    {
        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.4.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.4.
        /// </value>
        public Schema AdditionalProperties { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.3.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.3.
        /// </value>
        public IList<Schema> AllOf { get; set; }

        /// <summary>
        /// Gets or sets the value of the item that the server will use if none is provided. (Note: "default" has no meaning for required items.) See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-6.2. Unlike JSON Schema this value MUST conform to the defined type for the data type.
        /// </summary>
        /// <value>
        /// The value of the item that the server will use if none is provided.
        /// </value>
        public object Default { get; set; }

        /// <summary>
        /// Gets or sets the short description of the schema object.
        /// </summary>
        /// <value>
        /// The short description of the schema object.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Adds support for polymorphism. The discriminator is the schema property name that is used to differentiate between other schema that inherit this schema. The property name used MUST be defined at this schema and it MUST be in the required property list. When used, the value MUST be the name of this schema or any schema that inherits it..
        /// </summary>
        /// <value>
        /// The schema property name that is used to differentiate between other schema that inherit this schema.
        /// </value>
        public string Discriminator { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.1.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.1.
        /// </value>
        public string[] Enum { get; set; }

        /// <summary>
        /// Gets or sets the free-form property to include an example of an instance for this schema.
        /// </summary>
        /// <value>
        /// The free-form property to include an example of an instance for this schema.
        /// </value>
        public object Example { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.2.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.2.
        /// </value>
        public bool? ExclusiveMaximum { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.3.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.3.
        /// </value>
        public bool? ExclusiveMinimum { get; set; }

        /// <summary>
        /// Gets the JSON extension data.
        /// </summary>
        /// <value>
        /// The JSON extension data.
        /// </value>
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets additional external documentation for this schema.
        /// </summary>
        /// <value>
        /// The additional external documentation for this schema.
        /// </value>
        public ExternalDocs ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets the extending format for the previously mentioned type. See Data Type Formats for further details.
        /// </summary>
        /// <value>
        /// The extending format for the previously mentioned type. See Data Type Formats for further details.
        /// </value>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the type of items in the array.  Required if type is "array".
        /// </summary>
        /// <value>
        /// The type of items in the array.
        /// </value>
        public Schema Items { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.2.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.2.
        /// </value>
        public int? Maximum { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.3.2.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.3.2.
        /// </value>
        public int? MaxItems { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.1.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.1.
        /// </value>
        public int? MaxLength { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.3.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.3.
        /// </value>
        public int? Minimum { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.3.3.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.3.3.
        /// </value>
        public int? MinItems { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.2.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.2.
        /// </value>
        public int? MinLength { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.4.2.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.4.2.
        /// </value>
        public int? MinProperties { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.1.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.1.1.
        /// </value>
        public int? MultipleOf { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.3.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.3.
        /// </value>
        public string Pattern { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.4.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.2.4.
        /// </value>
        //[JsonConverter(typeof(DictionaryConverter))]
        public IDictionary<string, Schema> Properties { get; set; } = new SortedDictionary<string, Schema>();

        /// <summary>
        /// Relevant only for Schema "properties" definitions. Declares the property as "read only". This means that it MAY be sent as part of a response but MUST NOT be sent as part of the request. Properties marked as readOnly being true SHOULD NOT be in the required list of the defined schema. Default value is false.
        /// </summary>
        /// <value>
        /// A value indicating whether the schema is read only.
        /// </value>
        public bool? ReadOnly { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-pbryan-zyp-json-ref-03.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-pbryan-zyp-json-ref-03.
        /// </value>
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.4.3.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.4.3.
        /// </value>
        public IList<string> Required { get; set; }

        /// <summary>
        /// Gets or sets the title of the schema object.
        /// </summary>
        /// <value>
        /// The title of the schema object.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type of the object. The value MUST be one of "string", "number", "integer", "boolean", or "array".
        /// </summary>
        /// <value>
        /// The type of the object. The value MUST be one of "string", "number", "integer", "boolean", or "array".
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.3.4.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.3.4.
        /// </value>
        public bool? UniqueItems { get; set; }

        /// <summary>
        /// This MAY be used only on properties schemas. It has no effect on root schemas. Adds Additional metadata to describe the XML representation format of this property.
        /// </summary>
        /// <value>
        /// The metadata to describe the XML representation format of this property.
        /// </value>
        public Xml Xml { get; set; }
    }
}