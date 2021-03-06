/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Kuno.Configuration;
using Kuno.Services.Registry;
using Kuno.Text;
using Kuno.Utilities.NewId;
using Kuno.Validation;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// This is the root document object for the API specification. It combines what previously was the Resource Listing and API Declaration (version 1.2 and earlier) together into one document.
    /// </summary>
    /// <seealso href="http://swagger.io/specification/#swaggerObject"/>
    public class OpenApiDocument
    {
        /// <summary>
        /// Gets or sets the security scheme definitions that can be used across the specification.
        /// </summary>
        /// <value>
        /// The security scheme definitions that can be used across the specification.
        /// </value>
        public IDictionary<string, SecurityScheme> SecurityDefinitions { get; set; } = new SortedDictionary<string, SecurityScheme>
        {
            { "api_key", new SecurityScheme { Type = "apiKey", Name = "api_key", In = "header" } }
        };

        /// <summary>
        /// Gets or sets the base path on which the API is served, which is relative to the host. If it is not included, the API is served directly under the host. The value MUST start with a leading slash (/). The basePath does not support path templating.
        /// </summary>
        /// <value>
        /// The base path on which the API is served.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the object to hold data types produced and consumed by operations.
        /// </summary>
        /// <value>
        /// The object to hold data types produced and consumed by operations.
        /// </value>
        public SchemaCollection Definitions { get; set; } = new SchemaCollection();

        /// <summary>
        /// Gets or sets the additional external documentation.
        /// </summary>
        /// <value>
        /// The external additional external documentation.
        /// </value>
        public ExternalDocs ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets the host (name or ip) serving the API. This MUST be the host only and does not include the scheme nor sub-paths. It MAY include a port. If the host is not included, the host serving the documentation is to be used (including the port). The host does not support path templating.
        /// </summary>
        /// <value>
        /// The host (name or ip) serving the API.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        /// <value>
        /// The metadata about the API. The metadata can be used by the clients if needed.
        /// </value>
        public ApplicationInformation Info { get; set; }

        /// <summary>
        /// Gets or sets the available paths and operations for the API.
        /// </summary>
        /// <value>
        /// The available paths and operations for the API.
        /// </value>
        public IDictionary<string, PathItem> Paths { get; set; } = new SortedDictionary<string, PathItem>(new PathComparer());

        /// <summary>
        /// Gets or sets the transfer protocol of the API. Values MUST be from the list: "http", "https", "ws", "wss". If the schemes is not included, the default scheme to be used is the one used to access the Swagger definition itself.
        /// </summary>
        /// <value>
        /// The transfer protocol of the API. Values MUST be from the list: "http", "https", "ws", "wss". If the schemes is not included, the default scheme to be used is the one used to access the Swagger definition itself.
        /// </value>
        public string[] Schemes { get; set; } = { "http", "https" };

        /// <summary>
        /// Gets or sets the Swagger Specification version being used. It can be used by the Swagger UI and other clients to interpret the API listing. The value MUST be "2.0".
        /// </summary>
        /// <value>
        /// The Swagger Specification version being used. It can be used by the Swagger UI and other clients to interpret the API listing. The value MUST be "2.0".
        /// </value>
        public string Swagger { get; set; } = "2.0";

        /// <summary>
        /// Gets or sets the list of tags used by the specification with additional metadata. The order of the tags can be used to reflect on their order by the parsing tools. Not all tags that are used by the Operation Object must be declared. The tags that are not declared may be organized randomly or based on the tools' logic. Each tag name in the list MUST be unique.
        /// </summary>
        /// <value>
        /// The list of tags used by the specification with additional metadata.
        /// </value>
        public List<Tag> Tags { get; set; } = new List<Tag> { new Tag { Name = "Kuno", Description = "System defined endpoints." } };

        /// <summary>
        /// Loads the document using the specified service inventory.
        /// </summary>
        /// <param name="services">The service inventory.</param>
        /// <param name="includeAll">Indicates whether all endpoints should be retreived or just public.</param>
        /// <param name="versions">Indicates whether versioned paths should be returned.</param>
        public void Load(ServiceRegistry services, bool includeAll = false, bool versions = false)
        {
            this.Info = services.ApplicationInformation;
            var endPoints = services.EndPoints.Where(e => includeAll || e.Public).ToList();
            foreach (var group in endPoints.GroupBy(e => e.Path))
            {
                foreach (var endPoint in group)
                {
                    if (endPoint.Function.RequestType != null)
                    {
                        this.Definitions.GetOrAdd(endPoint.Function.RequestType);
                    }
                    if (endPoint.Function.ResponseType != null)
                    {
                        this.Definitions.GetOrAdd(endPoint.Function.ResponseType);
                    }

                    if (endPoint.Path != null)
                    {
                        if (versions)
                        {
                            this.Paths.Add("/" + endPoint.VersionedPath, new PathItem
                            {
                                Post = this.GetPostOperation(endPoint),
                                Get = this.GetGetOperation(endPoint)
                            });
                        }
                        else if (endPoint.Version == group.Max(e => e.Version))
                        {
                            this.Paths.Add("/" + endPoint.Path, new PathItem
                            {
                                Post = this.GetPostOperation(endPoint),
                                Get = this.GetGetOperation(endPoint)
                            });
                        }
                    }
                }
            }
        }

        private Operation GetGetOperation(EndPoint endPoint)
        {
            if (endPoint.HttpMethod == "GET")
            {
                var parameters = new List<IParameter>();
                foreach (var property in endPoint.Function.RequestType.GetProperties())
                {
                    var schema = this.Definitions.CreatePrimitiveSchema(property.PropertyType);
                    var required = property.GetCustomAttributes<ValidationAttribute>(true).Any();
                    parameters.Add(new NonBodyParameter
                    {
                        Name = property.Name,
                        Required = required,
                        In = "query",
                        Description = property.GetComments()?.Value,
                        Type = schema.Type,
                        Format = schema.Format
                    });
                }

                var operation = new Operation
                {
                    Tags = this.GetTags(endPoint).ToList(),
                    Summary = endPoint.Name,
                    Description = endPoint.Function.Summary,
                    Consumes = new List<string> { "application/json" },
                    Produces = new List<string> { "application/json" },
                    OperationId = this.GetName("GET_", endPoint),
                    Parameters = parameters,
                    Responses = this.GetResponses(endPoint)
                };

                if (operation.Responses.ContainsKey("401"))
                {
                    operation.IncludeSecurity("api_key");
                }

                if (endPoint.IsVersioned)
                {
                    operation.Deprecated = true;
                }

                return operation;
            }
            return null;
        }

        private readonly List<string> _usedNames = new List<string>();

        private string GetName(string prefix, EndPoint endPoint)
        {
            var regex = new Regex("[^a-zA-Z0-9]");

            var content = $"{prefix}_{regex.Replace(endPoint.Name, "")}_v{endPoint.Version}";

            if (_usedNames.Contains(content))
            {
                int i = 0;
                while (_usedNames.Contains(content + "_" + ++i))
                {
                }
                content = content + "_" + i;
            }
            _usedNames.Add(content);
            return content;

        }

        private Operation GetPostOperation(EndPoint endPoint)
        {
            if (endPoint.HttpMethod == "POST")
            {
                var operation = new Operation
                {
                    Tags = this.GetTags(endPoint).ToList(),
                    Summary = endPoint.Name,
                    Description = endPoint.Function.Summary,
                    Consumes = new List<string> { "application/json" },
                    Produces = new List<string> { "application/json" },
                    OperationId = this.GetName("POST_", endPoint),
                    Parameters = this.GetPostParameters(endPoint).ToList(),
                    Responses = this.GetResponses(endPoint)
                };

                if (operation.Responses.ContainsKey("401"))
                {
                    operation.IncludeSecurity("api_key");
                }

                if (endPoint.IsVersioned)
                {
                    operation.Deprecated = true;
                }

                return operation;
            }
            return null;
        }

        private IEnumerable<IParameter> GetPostParameters(EndPoint endPoint)
        {
            if (endPoint.Function.RequestType != null && endPoint.Function.RequestType != typeof(object))
            {
                yield return new BodyParameter
                {
                    Schema = this.Definitions.GetReferenceSchema(endPoint.Function.RequestType, endPoint.Function.RequestType.GetComments()?.Summary)
                };
            }
        }

        private Dictionary<string, Response> GetResponses(EndPoint endPoint)
        {
            var responses = new Dictionary<string, Response>();
            if (endPoint.Function.ResponseType == null)
            {
                responses.Add("204", new Response
                {
                    Description = "No content is returned from this endpoint."
                });
            }
            else
            {
                var responseType = endPoint.Function.ResponseType;
                responses.Add("200", new Response
                {
                    Description = responseType.GetComments()?.Summary ?? "",
                    Schema = this.Definitions.GetReferenceSchema(responseType, endPoint.Function.ResponseType.GetComments()?.Summary)
                });
            }
            var builder = new StringBuilder();
            foreach (var property in endPoint.Function.RequestType.GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes<ValidationAttribute>(true))
                {
                    builder.AppendLine("1. " + attribute.GetValidationError(property).Message + "\r\n");
                }
            }
            foreach (var source in endPoint.Function.Rules.Where(e => e.RuleType == ValidationType.Input))
            {
                builder.AppendLine(source.Name.ToTitle() + ".  ");
            }
            if (builder.Length > 0)
            {
                responses.Add("400", new Response
                {
                    Schema = this.Definitions.GetReferenceSchema(typeof(ValidationError[]), null),
                    Description = builder.ToString()
                });
            }
            builder.Clear();

            if (endPoint.Secure)
            {
                responses.Add("401", new Response
                {
                    Schema = this.Definitions.GetReferenceSchema(typeof(ValidationError[]), null),
                    Description = "This endpoint requires authorization."
                });
            }

            foreach (var source in endPoint.Function.Rules.Where(e => e.RuleType == ValidationType.Business))
            {
                builder.AppendLine("1. " + source.Name.ToTitle() + ".\r\n");
            }
            if (builder.Length > 0)
            {
                responses.Add("409", new Response
                {
                    Schema = this.Definitions.GetReferenceSchema(typeof(ValidationError[]), null),
                    Description = builder.ToString()
                });
            }
            builder.Clear();
            foreach (var source in endPoint.Function.Rules.Where(e => e.RuleType == ValidationType.Security))
            {
                builder.AppendLine("1. " + source.Name.ToTitle() + ".\r\n");
            }
            if (builder.Length > 0)
            {
                if (!responses.ContainsKey("401"))
                {
                    responses.Add("401", new Response
                    {
                        Schema = this.Definitions.GetReferenceSchema(typeof(ValidationError[]), null),
                        Description = "This endpoint requires authorization."
                    });
                }
                responses.Add("403", new Response
                {
                    Schema = this.Definitions.GetReferenceSchema(typeof(ValidationError[]), null),
                    Description = builder.ToString()
                });
            }

            return responses;
        }

        private IEnumerable<string> GetTags(EndPoint endPoint)
        {
            if (endPoint.Path == null)
            {
                yield break;
            }

            if (endPoint.Path.StartsWith("_") || endPoint.IsVersioned && endPoint.Path.Split('/').ElementAt(1)?.StartsWith("_") == true)
            {
                yield return "System";
                yield break;
            }

            if (endPoint.Tags != null)
            {
                foreach (var tag in endPoint.Tags)
                {
                    yield return tag;
                }
                yield break;
            }

            var segments = endPoint.Path.Split('/');
            if (segments.Length >= 3)
            {
                if (endPoint.IsVersioned)
                {
                    yield return segments[2].Replace("-", " ").ToTitle();
                }
                else
                {
                    yield return segments[1].Replace("-", " ").ToTitle();
                }
            }
        }

        private class PathComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                var left = new EndPointPath(x);
                var right = new EndPointPath(y);

                return left.CompareTo(right);
            }
        }
    }
}