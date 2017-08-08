/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Kuno.AspNetCore.Middleware;
using Kuno.AspNetCore.Settings;
using Kuno.AspNetCore.Swagger;

namespace Kuno.AspNetCore.Messaging
{
    internal class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Stack.Use(e =>
            {
                e.Register(c => env).As<IHostingEnvironment>().SingleInstance();
            });
        }

        public static AspNetCoreOptions Options { get; set; }

        public static KunoStack Stack { get; set; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(Options.CorsOptions);

            app.UseCookieAuthentication(Options.GetCookieAuthenticationOptions());

            app.UseMiddleware<ApiKeyMiddleware>(Options);

            app.UseMvc();

            app.UseSwaggerUI();

            app.UseKuno(Stack);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Stack.Include(this);

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAssertion(e =>
                {
                    return true;
                })
                .AddAuthenticationSchemes(Options.CookieAuthentication.AuthenticationScheme)
                .Build();

            var mvc = services.AddMvc(setup =>
                              {
                                  setup.Filters.Add(new AuthorizeFilter(defaultPolicy));
                              })
                              .AddJsonOptions(options =>
                              {
                                  options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                              });

            foreach (var assembly in Stack.Assemblies)
            {
                mvc.AddApplicationPart(assembly);
            }

            Stack.Use(builder =>
            {
                builder.Populate(services);
            });

            return new AutofacServiceProvider(Stack.Container);
        }
    }
}