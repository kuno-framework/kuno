using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Kuno.Security;
using Kuno.Serialization;
using Kuno.Services;
using Newtonsoft.Json;

namespace Kuno.AspNetCore.EndPoints.GenerateApiKey
{
    [EndPoint("_system/api-key/create", Name = "Create API Key", Public = false)]
    public class GenerateApiKey : Function<GenerateApiKeyRequest, string>
    {
        public override string Receive(GenerateApiKeyRequest instance)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, instance.UserName),
                new Claim(ClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(15).ToString(CultureInfo.InvariantCulture))
            };
            claims.AddRange(instance.Roles.Select(e => new Claim(ClaimTypes.Role, e)));

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "api_key"));

            var content = JsonConvert.SerializeObject(principal, DefaultSerializationSettings.Instance);

            return Encryption.Encrypt(content, instance.Key);
        }
    }
}
