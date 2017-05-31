/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// A message that contains document information.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document" /> class.
        /// </summary>
        /// <param name="name">The file name of the document.</param>
        /// <param name="content">The document content.</param>
        public Document(string name, byte[] content)
        {
            this.Name = name;
            this.Content = content;
        }

        /// <summary>
        /// Gets the document content.
        /// </summary>
        /// <value>The document content.</value>
        public byte[] Content { get; }

        /// <summary>
        /// Gets the file name of the document.
        /// </summary>
        /// <value>The file name of the document.</value>
        public string Name { get; }
    }
}