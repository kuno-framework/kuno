/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.Services;
using Microsoft.Extensions.Configuration;

namespace Kuno.EntityFramework.EndPoints.GetConfiguration
{
    /// <summary>
    /// Gets the current Entity Framework configuration.
    /// </summary>
    [EndPoint("_system/configuration/entity-framework", Method = "GET", Name = "Get Entity Framework Configuration", Public = false)]
    public class GetConfiguration : Function<GetConfigurationRequest, EntityFrameworkOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfiguration" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public GetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public override EntityFrameworkOptions Receive(GetConfigurationRequest instance)
        {
            var options = new EntityFrameworkOptions();
            _configuration.GetSection("Kuno:EntityFramework").Bind(options);

            return options;
        }
    }
}