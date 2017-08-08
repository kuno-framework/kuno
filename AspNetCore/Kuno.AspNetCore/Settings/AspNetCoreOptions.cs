/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Kuno.Security;
using Kuno.Text;

namespace Kuno.AspNetCore.Settings
{
    /// <summary>
    /// Options for AspNetCore.
    /// </summary>
    public class AspNetCoreOptions
    {
        /// <summary>
        /// Gets or sets the API key authentication settings.
        /// </summary>
        /// <value>
        /// The API key authentication settings.
        /// </value>
        public ApiKeyAuthenticationSettings ApiKeyAuthentication { get; set; } = new ApiKeyAuthenticationSettings();

        /// <summary>
        /// Gets or sets the cookie authentication settings.
        /// </summary>
        /// <value>The cookie authentication settings.</value>
        public CookieAuthenticationSettings CookieAuthentication { get; set; } = new CookieAuthenticationSettings();

        /// <summary>
        /// Gets or sets the subscription settings.
        /// </summary>
        /// <value>The subscription settings.</value>
        public SubscriptionSettings Subscriptions { get; set; } = new SubscriptionSettings();

        internal Action<CorsPolicyBuilder> CorsOptions { get; set; } = builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .Build();
        };

        internal string[] Urls { get; set; }

        /// <summary>
        /// Sets the CORS configuration.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This instance for method chaining.</returns>
        public AspNetCoreOptions WithCors(Action<CorsPolicyBuilder> options)
        {
            this.CorsOptions = options;

            return this;
        }

        /// <summary>
        /// Sets the URLs to use with local hosting.
        /// </summary>
        /// <param name="urls">The urls to use.</param>
        /// <returns>This instance for method chaining.</returns>
        public AspNetCoreOptions WithUrls(params string[] urls)
        {
            this.Urls = urls;

            return this;
        }

        internal CookieAuthenticationOptions GetCookieAuthenticationOptions()
        {
            return new CookieAuthenticationOptions
            {
                AuthenticationScheme = this.CookieAuthentication.AuthenticationScheme,
                CookieName = this.CookieAuthentication.CookieName,
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromSeconds(1),
                DataProtectionProvider = new CookieDataProtectionProvider(this.CookieAuthentication.DataProtectionKey),
                Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = a =>
                    {
                        a.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult(0);
                    },
                    OnValidatePrincipal = async a =>
                    {
                        if (a.Properties.ExpiresUtc <= DateTime.UtcNow)
                        {
                            await a.HttpContext.Authentication.SignOutAsync(this.CookieAuthentication.AuthenticationScheme);
                        }
                        else
                        {
                            a.ShouldRenew = true;
                        }
                    }
                }
            };
        }

        internal class CookieDataProtector : IDataProtector
        {
            private readonly string _salt;

            public CookieDataProtector(string salt)
            {
                _salt = salt;
            }

            public IDataProtector CreateProtector(string purpose)
            {
                return new CookieDataProtector(_salt);
            }

            public byte[] Protect(byte[] plaintext)
            {
                return Encrypt(plaintext, _salt);
            }

            public byte[] Unprotect(byte[] protectedData)
            {
                return Decrypt(protectedData, _salt);
            }

            #region Non-Public Methods

            private static void SetValidKey(SymmetricAlgorithm provider, string key)
            {
                key = key ?? "";
                provider.GenerateKey();
                provider.Key = Encoding.ASCII.GetBytes(key.Resize(provider.Key.Length, ' '));
            }

            [SuppressMessage("ReSharper", "InconsistentNaming")]
            private static void SetValidIV(SymmetricAlgorithm provider, string key)
            {
                key = key ?? "";
                provider.GenerateIV();
                provider.IV = Encoding.ASCII.GetBytes(key.Resize(provider.IV.Length, ' '));
            }

            private static byte[] Encrypt(byte[] text, string key)
            {
                using (var provider = Aes.Create())
                {
                    SetValidKey(provider, key);
                    SetValidIV(provider, key);
                    provider.Padding = PaddingMode.None;

                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(memoryStream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                        cryptoStream.Write(text, 0, text.Length);
                        return memoryStream.ToArray();
                    }
                }
            }

            private static byte[] Decrypt(byte[] text, string key)
            {
                using (var provider = Aes.Create())
                {
                    SetValidKey(provider, key);
                    SetValidIV(provider, key);
                    provider.Padding = PaddingMode.None;


                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, provider.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(text, 0, text.Length);
                            return memoryStream.ToArray();
                        }
                    }
                }
            }

            #endregion
        }

        internal class CookieDataProtectionProvider : IDataProtectionProvider
        {
            private readonly string _key;

            public CookieDataProtectionProvider(string key)
            {
                _key = key;
            }

            public IDataProtector CreateProtector(string purpose)
            {
                return new CookieDataProtector(_key);
            }
        }



    }
}