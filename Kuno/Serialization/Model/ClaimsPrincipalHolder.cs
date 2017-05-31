/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using System.Security.Claims;
using Kuno.Validation;

namespace Kuno.Serialization.Model
{
    /// <summary>
    /// EndPointType used to hold a <see cref="ClaimsPrincipal" /> for serialization.
    /// </summary>
    public class ClaimsPrincipalHolder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsPrincipalHolder" /> class.
        /// </summary>
        public ClaimsPrincipalHolder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsPrincipalHolder" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="source" /> argument is null.</exception>
        public ClaimsPrincipalHolder(ClaimsPrincipal source)
        {
            Argument.NotNull(source, nameof(source));

            this.AuthenticationType = source.Identity.AuthenticationType;
            this.Claims = source.Claims.Select(x => new ClaimHolder {Type = x.Type, Value = x.Value}).ToArray();
        }

        /// <summary>
        /// Gets or sets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>The claims.</value>
        public ClaimHolder[] Claims { get; set; }
    }
}