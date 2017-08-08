/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Kuno.Services;

namespace Kuno.AspNetCore.EndPoints
{
    /// <summary>
    /// Gets the request IP address.  Used for troubleshooting or to help services identify the IP address that they call from.
    /// </summary>
    [EndPoint("_system/ip-address", Method = "GET", Public = false, Name = "Get IP Address")]
    public class GetIPAddress : Function
    {
        /// <inheritdoc />
        public override void Receive()
        {
            this.Respond(this.Request.SourceAddress);
        }
    }
}