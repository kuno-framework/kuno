/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.AspNetCore.Messaging;
using Kuno.Services;
using Kuno.Validation;

namespace Kuno.AspNetCore.EndPoints
{
    /// <summary>
    /// Creates an event subscription and published raised events to the specified URL.
    /// </summary>
    [EndPoint("_system/events/subscribe", Public = false)]
    public class Subscribe : Function<SubscribeRequest>
    {
        private readonly HttpEventPublisher _publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscribe" /> class.
        /// </summary>
        /// <param name="publisher">The configured <see cref="HttpEventPublisher"/>.</param>
        public Subscribe(HttpEventPublisher publisher)
        {
            Argument.NotNull(publisher, nameof(publisher));

            _publisher = publisher;
        }

        /// <inheritdoc />
        public override void Receive(SubscribeRequest instance)
        {
            _publisher.Subscribe(instance.Url);
        }
    }
}