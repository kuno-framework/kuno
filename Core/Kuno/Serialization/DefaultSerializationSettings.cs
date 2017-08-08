/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Newtonsoft.Json;

namespace Kuno.Serialization
{
    /// <summary>
    /// Specifies the default messaging settings.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonSerializerSettings" />
    public class DefaultSerializationSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSerializationSettings" /> class.
        /// </summary>
        public DefaultSerializationSettings()
        {
            this.Formatting = Formatting.Indented;
            this.NullValueHandling = NullValueHandling.Ignore;
            this.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            this.ContractResolver = new DefaultContractResolver();
            this.Converters.Add(new ClaimsPrincipalConverter());
        }

        /// <summary>
        /// Gets an instance of the settings.
        /// </summary>
        /// <value>An instance of the settings.</value>
        public static JsonSerializerSettings Instance => new DefaultSerializationSettings();
    }
}