using System;

namespace Kuno.Services.EndPoints
{
    /// <summary>
    /// Request to get the OpenAPI definition.
    /// </summary>
    public class GetOpenApiRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOpenApiRequest" /> class.
        /// </summary>
        /// <param name="host">The host name to display in the document.</param>
        /// <param name="basePath">The base path on which the API is served.</param>
        /// <param name="all"><c>true</c> if all endpoints should be retrieved; otherwise, <c>false</c>.</param>
        public GetOpenApiRequest(string host, string basePath = null, bool all = false)
        {
            this.Host = host;
            this.All = all;
            this.BasePath = basePath;
        }

        /// <summary>
        /// Gets a value indicating whether all endpoints should be retrieved or just public ones.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all endpoints should be retrieved; otherwise, <c>false</c>.
        /// </value>
        public bool All { get; }

        /// <summary>
        /// Gets or sets the base path on which the API is served, which is relative to the host. If it is not included, the API is served directly under the host. The value MUST start with a leading slash (/). The basePath does not support path templating.
        /// </summary>
        /// <value>
        /// The base path on which the API is served.
        /// </value>
        public string BasePath { get; }

        /// <summary>
        /// Gets the host name to display in the document.
        /// </summary>
        /// <value>
        /// The host name to display in the document.
        /// </value>
        public string Host { get; }
    }
}