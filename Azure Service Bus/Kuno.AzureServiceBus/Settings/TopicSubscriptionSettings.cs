namespace Kuno.AzureServiceBus.Settings
{
    /// <summary>
    /// Settings for a topic subscription.
    /// </summary>
    public class TopicSubscriptionSettings
    {
        /// <summary>
        /// Gets or sets the name of the subscription.
        /// </summary>
        /// <value>
        /// The name of the subscription.
        /// </value>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the topic.
        /// </summary>
        /// <value>
        /// The name of the topic.
        /// </value>
        public string TopicName { get; set; }
    }
}