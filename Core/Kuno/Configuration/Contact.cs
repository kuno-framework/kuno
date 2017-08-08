/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Configuration
{
    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#contactObject"/>
    public class Contact
    {
        /// <summary>
        /// Gets or sets the email address of the contact person/organization. MUST be in the format of an email address.
        /// </summary>
        /// <value>
        /// The email address of the contact person/organization. MUST be in the format of an email address.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the identifying name of the contact person/organization.
        /// </summary>
        /// <value>
        /// The identifying name of the contact person/organization.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL pointing to the contact information. MUST be in the format of a URL.
        /// </summary>
        /// <value>
        /// The URL pointing to the contact information. MUST be in the format of a URL.
        /// </value>
        public string Url { get; set; }
    }
}