/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Kuno.Services.Messaging;

namespace Kuno.AspNetCore.Messaging
{
    /// <summary>
    /// An HTTP based <see cref="IEventPublisher" />.
    /// </summary>
    public class HttpEventPublisher : IEventPublisher
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly List<string> _urls = new List<string>();

        /// <inheritdoc />
        public Task Publish(params EventMessage[] events)
        {
            if (events.Any() && _urls.Any())
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var content = JsonConvert.SerializeObject(new
                {
                    Content = JsonConvert.SerializeObject(events, settings)
                }, settings);
                var body = new StringContent(content, Encoding.UTF8, "application/json");

                return Task.WhenAll(_urls.Select(e => _client.PostAsync(e + "/_system/events/publish", body)));
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Creates a subscription using the specified URL.
        /// </summary>
        /// <param name="url">The URL that will be published to.</param>
        public void Subscribe(string url)
        {
            url = url.TrimEnd('/');
            if (!_urls.Contains(url))
            {
                _urls.Add(url);
            }
        }
    }
}