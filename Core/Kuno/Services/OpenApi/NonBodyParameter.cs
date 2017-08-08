using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// A parameter that is not in the body of the API request.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#parameterObject"/>
    public class NonBodyParameter : PartialSchema, IParameter
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string In { get; set; }

        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        public bool Required { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; } = new Dictionary<string, object>();
    }
}