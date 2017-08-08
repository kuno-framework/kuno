using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Kuno.AspNetCore.Messaging;

namespace Kuno.AspNetCore.Swagger.UI.Application
{
    /// <summary>
    /// The index.html file for Swagger UI.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IFileInfo" />
    public class SwaggerUIIndexFileInfo : IFileInfo
    {
        private readonly Assembly _assembly;
        private readonly string _resourcePath;

        private long? _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerUIIndexFileInfo"/> class.
        /// </summary>
        /// <param name="assembly">The assembly containing the resources.</param>
        /// <param name="resourcePath">The resource path.</param>
        public SwaggerUIIndexFileInfo(Assembly assembly, string resourcePath)
        {
            _assembly = assembly;
            _resourcePath = resourcePath;
        }

        /// <inheritdoc />
        public bool Exists => true;

        /// <inheritdoc />
        public long Length
        {
            get
            {
                if (!_length.HasValue)
                {
                    using (var stream = this.CreateParameterizedStream())
                    {
                        _length = stream.Length;
                    }
                }
                return _length.Value;
            }
        }

        /// <inheritdoc />
        public string PhysicalPath => null;

        /// <inheritdoc />
        public string Name => "index.html";

        /// <inheritdoc />
        public DateTimeOffset LastModified => DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public bool IsDirectory => false;

        /// <inheritdoc />
        public Stream CreateReadStream()
        {
            var stream = this.CreateParameterizedStream();
            if (!_length.HasValue)
            {
                _length = stream.Length;
            }
            return stream;
        }

        private Stream CreateParameterizedStream()
        {
            var names = _assembly.GetManifestResourceNames().ToList();
            using (var templateStream = _assembly.GetManifestResourceStream(_resourcePath))
            {
                using (var reader = new StreamReader(templateStream))
                {
                    var templateText = reader.ReadToEnd();
                    var parameterizedTextBuilder = new StringBuilder(templateText);

                    var request = Startup.Stack.Container.Resolve<IHttpContextAccessor>().HttpContext.Request;

                    var path = $"{request.Scheme}://{request.Host}/{request.PathBase}".TrimEnd('/') + "/_system/api";
                    if (request.PathBase.HasValue)
                    {
                        path += "?basepath=" + request.PathBase;
                    }

                    parameterizedTextBuilder.Replace("{{URL}}", path);

                    return new MemoryStream(Encoding.UTF8.GetBytes(parameterizedTextBuilder.ToString()));
                }
            }
        }
    }
}