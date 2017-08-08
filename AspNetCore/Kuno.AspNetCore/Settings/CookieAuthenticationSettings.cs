using System;

namespace Kuno.AspNetCore.Settings
{
    /// <summary>
    /// Settings for cookie authentication..
    /// </summary>
    public class CookieAuthenticationSettings
    {
        /// <summary>
        /// Gets or sets the authentication scheme name.
        /// </summary>
        /// <value>The authentication scheme name.</value>
        public string AuthenticationScheme { get; set; } = "Cookies";

        /// <summary>
        /// Gets or sets the name of the cookie.
        /// </summary>
        /// <value>The name of the cookie.</value>
        public string CookieName { get; set; } = ".AspNetCore.Cookies";

        /// <summary>
        /// Gets or sets the data protection key that is used to encrypt the cookie.
        /// </summary>
        /// <value>The data protection key that is used to encrypt the cookie.</value>
        public string DataProtectionKey { get; set; } = @"Kuno";

        /// <summary>
        /// Gets or sets the expire time span.
        /// </summary>
        /// <value>The expire time span.</value>
        public TimeSpan ExpireTimeSpan { get; set; } = TimeSpan.FromMinutes(15);
    }

}