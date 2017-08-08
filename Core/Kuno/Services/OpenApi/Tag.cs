using System;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// Allows adding meta data to a single tag that is used by the Operation Object. It is not mandatory to have a Tag Object per tag used there.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#tagObject"/>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the short description for the tag. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>
        /// The short description for the tag. GFM syntax can be used for rich text representation.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the additional external documentation for this tag.
        /// </summary>
        /// <value>
        /// The external additional external documentation for this tag.
        /// </value>
        public ExternalDocs ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        public string Name { get; set; }
    }
}