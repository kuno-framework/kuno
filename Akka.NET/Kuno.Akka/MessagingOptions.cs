/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Akka
{
    /// <summary>
    /// Options for Akka.NET Messaging.
    /// </summary>
    public class MessagingOptions
    {
        internal string[] Remotes { get; set; } = new string[0];

        internal string SystemName { get; set; } = "local";

        internal bool UseLoggingClient { get; set; }

        /// <summary>
        /// Configures the stack to use the Akka.NET actor system with the specified name.
        /// </summary>
        /// <param name="name">The actor system name.</param>
        /// <returns>This instance for method chaining.</returns>
        public MessagingOptions WithName(string name)
        {
            this.SystemName = name;
            return this;
        }

        /// <summary>
        /// Configures the stack to use the Akka.NET actor system to connect to the specified remote systems.
        /// </summary>
        /// <returns>This instance for method chaining.</returns>
        public MessagingOptions WithRemotes(params string[] remotes)
        {
            this.Remotes = remotes;

            return this;
        }
    }
}