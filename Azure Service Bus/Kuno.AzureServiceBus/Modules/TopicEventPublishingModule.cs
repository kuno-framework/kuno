/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Autofac;
using Kuno.AzureServiceBus.Components;
using Kuno.AzureServiceBus.Settings;
using Kuno.Validation;

namespace Kuno.AzureServiceBus.Modules
{
    /// <summary>
    /// Module to configure Azure Topic event publishing.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class TopicEventPublishingModule : Module
    {
        private readonly AzureServiceBusSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicEventPublishingModule" /> class.
        /// </summary>
        /// <param name="settings">The current settings.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="settings"/> argument is null.</exception>
        public TopicEventPublishingModule(AzureServiceBusSettings settings)
        {
            Argument.NotNull(settings, nameof(settings));

            _settings = settings;
        }

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TopicEventPublisher>()
                   .WithParameter(new TypedParameter(typeof(AzureServiceBusSettings), _settings))
                   .AsSelf().AsImplementedInterfaces().SingleInstance();
        }
    }
}