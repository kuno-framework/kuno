/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Security.Principal;

namespace Kuno.Security
{
    /// <summary>
    /// Represents an anonymous identity.  Intended to implement the null object pattern.
    /// </summary>
    /// <seealso href="http://bit.ly/29e2gRR">Wikipedia: Null Object pattern</seealso>
    /// <seealso cref="System.Security.Principal.GenericIdentity" />
    public class AnonymousIdentity : GenericIdentity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousIdentity" /> class.
        /// </summary>
        public AnonymousIdentity()
            : base("")
        {
        }

        /// <summary>
        /// Gets a value indicating whether the user has been authenticated.
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public override bool IsAuthenticated => false;

        /// <summary>
        /// Gets the user's name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => null;
    }
}