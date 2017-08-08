/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Kuno.AspNetCore.Settings;
using Kuno.Security;
using Kuno.Serialization;

namespace Kuno.AspNetCore.Middleware
{
    /// <summary>
    /// Middleware that accepts an API key for authentication.
    /// </summary>
    /// <seealso cref="ApiKeyAuthenticationSettings"/>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AspNetCoreOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        /// <param name="options">The options for AspNetCore.</param>
        public ApiKeyMiddleware(RequestDelegate next, AspNetCoreOptions options)
        {
            _next = next;
            _options = options;
        }

        private static string RemoveControlCharacters(string text)
        {
            if (text == null)
            {
                return null;
            }
            var builder = new StringBuilder();
            foreach (var ch in text)
            {
                if (!char.IsControl(ch))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Invokes the middleware using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Return a task for asychronous programming.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (_options.ApiKeyAuthentication.Allow && context.Request.Headers.ContainsKey("api_key"))
            {
                var value = context.Request.Headers["api_key"].FirstOrDefault();
                try
                {
                    var text = RemoveControlCharacters(Encryption.Decrypt(value, _options.ApiKeyAuthentication.Key));

                    var current = JsonConvert.DeserializeObject<ClaimsPrincipal>(text, DefaultSerializationSettings.Instance);
                    var identity = current.Identity as ClaimsIdentity;
                    if (identity != null && identity.AuthenticationType == "api_key")
                    {
                        var expiration = identity.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Expiration);
                        if (expiration == null || DateTimeOffset.Parse(expiration.Value) > DateTimeOffset.Now)
                        {
                            context.User = current;
                        }
                    }
                }
                catch
                {
                    context.User = null;
                }
            }
            await _next.Invoke(context);
        }
    }
}