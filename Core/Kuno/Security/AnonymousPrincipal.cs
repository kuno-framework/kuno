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
    /// Represents an anonymous principal.  Intended to implement the null object pattern.
    /// </summary>
    /// <seealso cref="System.Security.Principal.GenericPrincipal" />
    /// <seealso href="http://bit.ly/29e2gRR">Wikipedia: Null Object pattern</seealso>
    public class AnonymousPrincipal : GenericPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousPrincipal" /> class.
        /// </summary>
        public AnonymousPrincipal()
            : base(new AnonymousIdentity(), new string[0])
        {
        }
    }
}