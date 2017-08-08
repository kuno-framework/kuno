using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Kuno.AspNetCore.Swagger.UI.Application;

namespace Kuno.AspNetCore.Swagger
{
    /// <summary>
    /// Swagger UI builder extensions to configure an application.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger UI to the application.
        /// </summary>
        /// <param name="app">The application to configure.</param>
        /// <returns>The instance for method chainging.</returns>
        // ReSharper disable once InconsistentNaming
        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        {
            var options = new FileServerOptions
            {
                RequestPath = "/swagger",
                FileProvider = new SwaggerUIFileProvider(),
                EnableDefaultFiles = true
            };
            options.StaticFileOptions.ContentTypeProvider = new FileExtensionContentTypeProvider();

            app.UseFileServer(options);

            return app;
        }
    }
}