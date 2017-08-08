/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.Serialization;
using Newtonsoft.Json;

namespace Kuno.Services.Messaging
{
    /// <summary>
    /// Contains extension methods to messages.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Gets the body of the message as the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns the body of the message as the specified type.</returns>
        public static T GetBody<T>(this IMessage instance)
        {
            return JsonConvert.DeserializeObject<T>(instance?.Body, DefaultSerializationSettings.Instance);
        }
    }
}