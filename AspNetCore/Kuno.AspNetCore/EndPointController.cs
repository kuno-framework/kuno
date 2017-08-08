/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Kuno.AspNetCore.Messaging;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.AspNetCore
{
    // TODO: Find a common method for requests and responses

    /// <summary>
    /// Base controller to support decorating Kuno endpoints.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public abstract class EndPointController : Controller
    {
        /// <summary>
        /// Executes the endpoints send method.
        /// </summary>
        /// <returns></returns>
        protected async Task Send()
        {
            var context = this.Request.HttpContext;
            var endPoint = this.Request.GetEndPoint(Startup.Stack);
            if (endPoint != null)
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else if (context.Request.Method == "GET")
                {
                    if (context.Request.Query.Any())
                    {
                        var content = new JObject();
                        foreach (var item in context.Request.Query)
                        {
                            content.Add(item.Key, item.Value.ToString());
                        }
                        var result = await Startup.Stack.Send(endPoint.Path, content.ToString());
                        HandleResult(result, context);
                    }
                    else
                    {
                        var result = await Startup.Stack.Send(endPoint.Path);
                        HandleResult(result, context);
                    }
                }
                else if (context.Request.Method == "POST")
                {
                    using (var stream = new MemoryStream())
                    {
                        context.Request.Body.CopyTo(stream);

                        var content = Encoding.UTF8.GetString(stream.ToArray());
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            content = null;
                        }
                        var result = await Startup.Stack.Send(endPoint.Path, content);
                        HandleResult(result, context);
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                }
            }
        }

        private static void HandleResult(MessageResult result, HttpContext context)
        {
            if (result.ValidationErrors.Any(e => e.Type == ValidationType.Input))
            {
                Respond(context, result.ValidationErrors, HttpStatusCode.BadRequest);
            }
            else if (result.ValidationErrors.Any(e => e.Type == ValidationType.Security))
            {
                Respond(context, result.ValidationErrors, HttpStatusCode.Unauthorized);
            }
            else if (result.ValidationErrors.Any())
            {
                Respond(context, result.ValidationErrors, HttpStatusCode.Conflict);
            }
            else if (!result.IsSuccessful)
            {
                var message = "An unhandled exception was raised on the server.  Please try again.  " + result.CorrelationId;
                Respond(context, message, HttpStatusCode.InternalServerError);
            }
            else if (result.Response != null)
            {
                if (result.Response is Document)
                {
                    Respond(context, (Document)result.Response, HttpStatusCode.OK);
                }
                else
                {
                    Respond(context, result.Response, HttpStatusCode.OK);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
        }

        private static void Respond(HttpContext context, Document content, HttpStatusCode statusCode)
        {
            using (var stream = new MemoryStream(content.Content))
            {
                context.Response.ContentType = MimeTypes.GetMimeType(Path.GetExtension(content.Name));
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentLength = content.Content.Length;
                stream.CopyTo(context.Response.Body);
            }
        }

        private static void Respond(HttpContext context, object content, HttpStatusCode statusCode)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            using (var inner = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content, settings))))
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentLength = inner.ToArray().Length;
                inner.CopyTo(context.Response.Body);
            }
        }
    }
}