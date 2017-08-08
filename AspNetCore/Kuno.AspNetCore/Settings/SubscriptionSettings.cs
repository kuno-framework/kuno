namespace Kuno.AspNetCore.Settings
{
    /// <summary>
    /// Settings for subscriptions.
    /// </summary>
    public class SubscriptionSettings
    {
        /// <summary>
        /// Gets or sets the local URL of the service that will be called on publish.
        /// </summary>
        /// <value>The local URL of the service that will be called on publish.</value>
        public string Local { get; set; }

        /// <summary>
        /// Gets or sets the remote URLs to subscribe to.
        /// </summary>
        /// <value>The remote URLs to subscribe to.</value>
        public string[] Remote { get; set; } = new string[0];
    }
}