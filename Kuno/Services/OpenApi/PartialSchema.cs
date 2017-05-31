/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// A limited subset of JSON-Schema's items object. It is used by parameter definitions that are not located in "body"
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#itemsObject"/>
    public class PartialSchema
    {
        /// <summary>
        /// Gets or sets the format of the array if type array is used. Default value is csv.  Possible values are:
        /// <para>csv - comma separated values foo,bar.</para>
        /// <para>ssv - space separated values foo bar.</para>
        /// <para>tsv - tab separated values foo\tbar.</para>
        /// <para>pipes - pipe separated values foo|bar.</para>
        /// </summary>
        /// <value>
        /// The collection format of the array if type array is used.
        /// </value>
        public string CollectionFormat { get; set; }

        /// <summary>
        /// Gets or sets the value of the item that the server will use if none is provided. (Note: "default" has no meaning for required items.) See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-6.2. Unlike JSON Schema this value MUST conform to the defined type for the data type.
        /// </summary>
        /// <value>
        /// The value of the item that the server will use if none is provided.
        /// </value>
        public object Default { get; set; }

        /// <summary>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.1.
        /// </summary>
        /// <value>
        /// See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-5.5.1.
        /// </value>
        public List<object> Enum { get; set; } = new List<object>();

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
        public PartialSchema Items { get; set; }

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
    }
}