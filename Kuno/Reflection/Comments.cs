/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using System.Xml.Linq;

namespace Kuno.Reflection
{
    /// <summary>
    /// The XML comments of a code element.
    /// </summary>
    public class Comments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comments" /> class.
        /// </summary>
        /// <param name="node">The node containing the comments.</param>
        public Comments(XElement node)
        {
            this.ReadFromNode(node);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comments" /> class.
        /// </summary>
        /// <param name="comments">The test containing the comments.</param>
        public Comments(string comments)
        {
            var node = XElement.Parse(comments);
            this.ReadFromNode(node);
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        private void ReadFromNode(XElement node)
        {
            this.Summary = node.Descendants().FirstOrDefault(e => e.Name.LocalName == "summary")?.Value.Trim();
            this.Value = node.Descendants().FirstOrDefault(e => e.Name.LocalName == "value")?.Value.Trim();
        }
    }
}