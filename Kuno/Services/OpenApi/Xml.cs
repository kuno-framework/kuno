/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// A metadata object that allows for more fine-tuned XML model definitions. When using arrays, XML element names are not inferred(for singular/plural forms) and the name property should be used to add that information.See examples for expected behavior.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#xmlObject"/>
    public class Xml
    {
        /// <summary>
        /// Declares whether the property definition translates to an attribute instead of an element. Default value is false.
        /// </summary>
        /// <value>
        /// A value indicating whether the property definition translates to an attribute instead of an element.
        /// </value>
        public bool? Attribute { get; set; }

        /// <summary>
        /// Replaces the name of the element/attribute used for the described schema property. When defined within the Items Object (items), it will affect the name of the individual XML elements within the list. When defined alongside type being array (outside the items), it will affect the wrapping element and only if wrapped is true. If wrapped is false, it will be ignored.
        /// </summary>
        /// <value>
        /// The name of the element/attribute used for the described schema property.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL of the namespace definition. Value SHOULD be in the form of a URL.
        /// </summary>
        /// <value>
        /// The URL of the namespace definition. Value SHOULD be in the form of a URL.
        /// </value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the prefix to be used for the name.
        /// </summary>
        /// <value>
        /// The prefix to be used for the name.
        /// </value>
        public string Prefix { get; set; }

        /// <summary>
        /// MAY be used only for an array definition. Signifies whether the array is wrapped (for example, &lt;books&gt;&lt;book/&gt;&lt;book/&gt;&lt;/books&gt;) or unwrapped (&lt;book/&gt;&lt;book/&gt;). Default value is false. The definition takes effect only when defined alongside type being array (outside the items).
        /// </summary>
        /// <value>
        /// A value indicating whether the array is wrapped.
        /// </value>
        public bool? Wrapped { get; set; }
    }
}