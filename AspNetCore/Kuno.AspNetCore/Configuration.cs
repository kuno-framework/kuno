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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Kuno.AspNetCore.Messaging;
using Kuno.AspNetCore.Middleware;
using Kuno.AspNetCore.Settings;
using Kuno.Services;
using Kuno.Services.Messaging;
using Kuno.Services.Registry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using EndPoint = Kuno.Services.Registry.EndPoint;

namespace Kuno.AspNetCore
{
    /// <summary>
    /// Extension methods for configuration AspNetCore blocks.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Starts and runs an API to access the stack.
        /// </summary>
        /// <param name="stack">The this instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the instance for method chaining.</returns>
        public static KunoStack RunWebHost(this KunoStack stack, Action<AspNetCoreOptions> configuration = null)
        {
            stack.UseHttpMessaging();
            stack.Use(e =>
            {
                e.RegisterType<AspNetCoreRequestContext>().As<IRequestContext>().AsSelf();
            });

            var options = new AspNetCoreOptions();
            configuration?.Invoke(options);
            stack.Configuration.GetSection("Kuno:AspNetCore").Bind(options);

            Startup.Stack = stack;
            Startup.Options = options;

            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();

            if (options.Urls?.Any() ?? false)
            {
                builder.UseUrls(options.Urls);
            }

            var host = builder.Build();

            if (options.Subscriptions != null && options.Subscriptions.Remote.Any())
            {
                Task.Run(() => Subscribe(stack, options.Subscriptions));
            }

            host.Run();

            return stack;
        }

        /// <summary>
        /// Configures the stack to use HTTP messaging.
        /// </summary>
        /// <param name="stack">The this instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the instance for method chaining.</returns>
        public static KunoStack UseHttpMessaging(this KunoStack stack, Action<AspNetCoreOptions> configuration = null)
        {
            var options = new AspNetCoreOptions();
            configuration?.Invoke(options);
            stack.Configuration.GetSection("Kuno:AspNetCore").Bind(options);

            stack.Use(e =>
            {
                e.RegisterType<HttpRouter>().AsImplementedInterfaces().AsSelf().SingleInstance();
            });

            if (options.Subscriptions != null && options.Subscriptions.Remote.Any())
            {
                Subscribe(stack, options.Subscriptions);
            }

            return stack;
        }

        /// <summary>
        /// Configures the application to use Kuno.
        /// </summary>
        /// <param name="app">The application to configure.</param>
        /// <param name="stack">The current stack.</param>
        /// <returns>This instance for method chaining.</returns>
        public static IApplicationBuilder UseKuno(this IApplicationBuilder app, KunoStack stack)
        {
            return app.UseMiddleware<KunoMiddleware>(stack);
        }

        /// <summary>
        /// Gets an endpoint from the request.
        /// </summary>
        /// <param name="stack">The stack.</param>
        /// <param name="request">The current request.</param>
        /// <returns>Returns an endpoint from the request.</returns>
        internal static EndPoint GetEndPoint(this KunoStack stack, HttpRequest request)
        {
            var path = request.Path.Value.Trim('/');
            var inventory = stack.Container.Resolve<ServiceRegistry>();
            return inventory.EndPoints.Find(path);
        }

        /// <summary>
        /// Gets an endpoint from the request.
        /// </summary>
        /// <param name="stack">The stack.</param>
        /// <param name="request">The current request.</param>
        /// <returns>Returns an endpoint from the request.</returns>
        internal static EndPoint GetEndPoint(this HttpRequest request, KunoStack stack)
        {
            var path = request.Path.Value.Trim('/');
            var inventory = stack.Container.Resolve<ServiceRegistry>();
            return inventory.EndPoints.Find(path);
        }

        private static void Subscribe(KunoStack stack, SubscriptionSettings options)
        {
            var target = stack.Container.Resolve<RemoteServiceInventory>();
            using (var client = new HttpClient())
            {
                foreach (var url in options.Remote)
                {
                    if (!String.IsNullOrWhiteSpace(options.Local))
                    {
                        try
                        {
                            var message = new StringContent(JsonConvert.SerializeObject(new
                            {
                                url = options.Local
                            }), Encoding.UTF8, "application/json");
                            client.PostAsync(url + "/_system/events/subscribe", message).Wait();
                        }
                        catch (Exception exception)
                        {
                            stack.Logger.Warning(exception, $"Failed to subscribe to {url}");
                            continue;
                        }
                    }

                    try
                    {
                        var result = client.GetAsync(url + "/_system/endpoints").Result;
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var content = result.Content.ReadAsStringAsync().Result;
                            var endPoints = JsonConvert.DeserializeObject<RemoteService>(content);
                            target.Add(endPoints);
                        }
                    }
                    catch (Exception exception)
                    {
                        stack.Logger.Warning(exception, $"Failed to get remote endpoints at {url}");
                        continue;
                    }
                }
            }
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(15));

                Subscribe(stack, options);
            });
        }
    }
}