/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using Kuno.AzureServiceBus.Components;
using Kuno.AzureServiceBus.Modules;
using Kuno.AzureServiceBus.Settings;

namespace Kuno.AzureServiceBus
{
    /// <summary>
    /// Configures Azure Service Bus components.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Configures all the Azure Service Bus components.
        /// </summary>
        /// <param name="instance">The this instance.</param>
        /// <returns>A task for asynchronous programming.</returns>
        public static KunoStack UseAzureServiceBus(this KunoStack instance)
        {
            var settings = new AzureServiceBusSettings();
            instance.Configuration.GetSection("kuno:azureServiceBus")?.Bind(settings);

            instance.Use(builder =>
            {
                if (settings.EventPublisher?.TopicName != null)
                {
                    builder.RegisterModule(new TopicEventPublishingModule(settings));
                }
                if (settings.Subscriptions.Any())
                {
                    builder.RegisterModule(new TopicSubscriptionModule(settings));
                }
            });

            if (settings.Subscriptions.Any())
            {
                instance.Container.Resolve<TopicSubscription>();
            }

            return instance;
        }
    }
}