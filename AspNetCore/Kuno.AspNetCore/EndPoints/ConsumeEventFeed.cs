/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Kuno.Services;
using Kuno.Services.Messaging;

namespace Kuno.AspNetCore.EndPoints
{
    /// <summary>
    /// Requests events from a remote feed and then publishes them to any listeners locally.
    /// </summary>
    public class ConsumeEventFeed : Function<ConsumeEventFeedRequest>
    {
        private readonly IMessageGateway _messages;
        private readonly List<string> _received = new List<string>();
        private DateTimeOffset? _lastReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumeEventFeed" /> class.
        /// </summary>
        /// <param name="messages">The configured <see cref="IMessageGateway"/>.</param>
        public ConsumeEventFeed(IMessageGateway messages)
        {
            _messages = messages;
        }

        /// <inheritdoc />
        public override void Receive(ConsumeEventFeedRequest instance)
        {
            using (var client = new HttpClient())
            {
                var content = client.GetStringAsync(this.GetUrl(instance)).Result;

                var items = JsonConvert.DeserializeObject<JObject[]>(content);
                foreach (var item in items)
                {
                    var name = item["name"].Value<string>();
                    var id = item["id"].Value<string>();
                    if (!_received.Contains(id))
                    {
                        _received.Add(id);
                        _messages.Publish(name, item.ToString());

                        var last = DateTimeOffset.Parse(item["timeStamp"].Value<string>());
                        if (_lastReceived == null || last > _lastReceived)
                        {
                            _lastReceived = last;
                        }
                    }
                }
            }
        }

        private string GetUrl(ConsumeEventFeedRequest instance)
        {
            var root = instance.Url + "/_system/events";
            if (_lastReceived.HasValue)
            {
                root += "?start=" + _lastReceived.Value;
            }
            return root;
        }
    }
}