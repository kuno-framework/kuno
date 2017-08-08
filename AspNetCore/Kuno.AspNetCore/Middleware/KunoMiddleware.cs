/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Kuno.AspNetCore.Messaging;
using Kuno.Serialization;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.AspNetCore.Middleware
{
    /// <summary>
    /// Middleware that executes Kuno endpoints.
    /// </summary>
    public class KunoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly KunoStack _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="KunoMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        /// <param name="stack">The configured stack.</param>
        public KunoMiddleware(RequestDelegate next, KunoStack stack)
        {
            _next = next;
            _stack = stack;
        }

        /// <summary>
        /// Invokes the middleware using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Return a task for asychronous programming.</returns>
        public async Task Invoke(HttpContext context)
        {
            var endPoint = _stack.GetEndPoint(context.Request);
            if (endPoint != null)
            {
                try
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
                            var result = await _stack.Send(endPoint.Path, content.ToString());
                            HandleResult(result, context);
                        }
                        else
                        {
                            var result = await _stack.Send(endPoint.Path, null);
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
                            var result = await _stack.Send(endPoint.Path, content);
                            HandleResult(result, context);
                        }
                    }
                    else
                    {
                        await _next(context);
                    }
                }
                catch (Exception exception)
                {
                    _stack.Logger.Error(exception, "Failed to execute endpoint in the Kuno middleware.");
                    throw;
                }
            }
            else
            {
                await _next(context);
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
            using (var inner = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content, DefaultSerializationSettings.Instance))))
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentLength = inner.ToArray().Length;
                inner.CopyTo(context.Response.Body);
            }
        }
    }
}