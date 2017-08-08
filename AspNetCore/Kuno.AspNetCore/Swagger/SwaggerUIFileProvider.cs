using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Kuno.AspNetCore.Swagger.UI.Application
{
    /// <summary>
    /// The Swagger UI File provider.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class SwaggerUIFileProvider : IFileProvider
    {
        private const string StaticFilesNamespace = "Kuno.AspNetCore.bower_components.swagger_ui.dist";
        private const string IndexResourceName = "Kuno.AspNetCore.Swagger.index.html";
        private readonly EmbeddedFileProvider _staticFileProvider;

        private readonly Assembly _thisAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerUIFileProvider"/> class.
        /// </summary>
        public SwaggerUIFileProvider()
        {
            _thisAssembly = this.GetType().GetTypeInfo().Assembly;
            _staticFileProvider = new EmbeddedFileProvider(_thisAssembly, StaticFilesNamespace);
        }

        /// <inheritdoc />
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _staticFileProvider.GetDirectoryContents(subpath);
        }

        /// <inheritdoc />
        public IFileInfo GetFileInfo(string subpath)
        {
            if (subpath == "/index.html")
            {
                return new SwaggerUIIndexFileInfo(_thisAssembly, IndexResourceName);
            }

            return _staticFileProvider.GetFileInfo(subpath);
        }

        /// <inheritdoc />
        public IChangeToken Watch(string filter)
        {
            return _staticFileProvider.Watch(filter);
        }
    }
}