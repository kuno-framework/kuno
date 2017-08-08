/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using System.Security.Claims;
using Kuno.Services.Messaging;
using Microsoft.AspNetCore.Http;

namespace Kuno.AspNetCore.Messaging
{
    /// <summary>
    /// Provides a request context for AspNetCore.
    /// </summary>
    /// <seealso cref="Kuno.Services.Messaging.Request" />
    public class AspNetCoreRequestContext : RequestContext
    {
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetCoreRequestContext"/> class.
        /// </summary>
        /// <param name="accessor">The accessor.</param>
        public AspNetCoreRequestContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <inheritdoc />
        protected override string GetSession()
        {
            var context = _accessor.HttpContext;
            var key = Guid.NewGuid().ToString();
            var kunoSession = "kuno.session";
            if (context.Request.Cookies.ContainsKey(kunoSession))
            {
                key = context.Request.Cookies[kunoSession];
            }
            else
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Path = "/",
                    Secure = false
                };

                context.Response.Cookies.Append(kunoSession, key, cookieOptions);
            }
            return key;
        }

        /// <inheritdoc />
        protected override string GetSourceIPAddress()
        {
            var forward = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forward))
            {
                var target = forward.Split(',')[0];
                if (target.Contains("."))
                {
                    return target.Split(':')[0];
                }
            }
            return _accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        /// <inheritdoc />
        protected override ClaimsPrincipal GetUser()
        {
            return _accessor.HttpContext?.User;
        }
    }
}