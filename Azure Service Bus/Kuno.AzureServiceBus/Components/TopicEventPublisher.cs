/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Kuno.AzureServiceBus.Settings;
using Kuno.Serialization;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.AzureServiceBus.Components
{
    /// <summary>
    /// Publishes events to an Azure Topic.
    /// </summary>
    public class TopicEventPublisher : IEventPublisher
    {
        private readonly Lazy<TopicClient> _topicClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicEventPublisher" /> class.
        /// </summary>
        /// <param name="settings">The current settings.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="settings" /> argument is null.</exception>
        public TopicEventPublisher(AzureServiceBusSettings settings)
        {
            Argument.NotNull(settings, nameof(settings));

            _topicClient = new Lazy<TopicClient>(() => CreateTopic(settings));
        }

        private static TopicClient CreateTopic(AzureServiceBusSettings settings)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(settings.ConnectionString);

            if (!namespaceManager.TopicExists(settings.EventPublisher.TopicName))
            {
                namespaceManager.CreateTopic(settings.EventPublisher.TopicName);
            }
            return TopicClient.CreateFromConnectionString(settings.ConnectionString, settings.EventPublisher.TopicName);
        }

        /// <inheritdoc />
        public async Task Publish(params EventMessage[] events)
        {
            await _topicClient.Value.SendBatchAsync(events.Select(message =>
            {
                var content = JsonConvert.SerializeObject(message, DefaultSerializationSettings.Instance);
                return new BrokeredMessage(content)
                {
                    MessageId = message.Id,
                    ContentType = message.MessageType
                };
            }));
        }
    }
}