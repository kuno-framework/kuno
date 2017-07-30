using Kuno.Serialization;
using Newtonsoft.Json;

namespace Kuno.Services.Messaging
{
    public static class MessageExtensions
    {
        public static T GetBody<T>(this IMessage instance)
        {
            return JsonConvert.DeserializeObject<T>(instance.Body, DefaultSerializationSettings.Instance);
        }
    }
}