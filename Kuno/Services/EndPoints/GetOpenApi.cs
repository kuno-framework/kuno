using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Kuno.Services.Inventory;
using Kuno.Services.OpenApi;

namespace Kuno.Services.EndPoints
{
    /// <summary>
    /// Gets the [OpenAPI](https://www.openapis.org/) definition for the API.
    /// </summary>
    [EndPoint("_system/api", Method = "GET", Name = "Get OpenAPI Definition", Public = false)]
    public class GetOpenApi : EndPoint<GetOpenApiRequest, OpenApiDocument>
    {
        private readonly ServiceInventory _services;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOpenApi" /> class.
        /// </summary>
        /// <param name="services">The configured services.</param>
        /// <param name="configuration">The configuration.</param>
        public GetOpenApi(ServiceInventory services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public override OpenApiDocument Receive(GetOpenApiRequest instance)
        {
            var document = new OpenApiDocument();
            document.Load(_services, instance.All);
            document.Host = instance.Host;

            if (!String.IsNullOrWhiteSpace(instance.BasePath))
            {
                document.BasePath = instance.BasePath;
            }

            var externalDocs = new ExternalDocs();
            _configuration.GetSection("stacks:externalDocs").Bind(externalDocs);
            if (!String.IsNullOrWhiteSpace(externalDocs.Url))
            {
                document.ExternalDocs = externalDocs;
            }

            var tags = new List<Tag>();
            _configuration.GetSection("stacks:tags").Bind(tags);
            if (tags.Any())
            {
                document.Tags.AddRange(tags);
            }

            return document;
        }
    }
}