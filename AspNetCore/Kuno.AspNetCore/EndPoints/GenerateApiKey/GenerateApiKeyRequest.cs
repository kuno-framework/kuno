using System;

namespace Kuno.AspNetCore.EndPoints.GenerateApiKey
{
    public class GenerateApiKeyRequest
    {
        public GenerateApiKeyRequest(string key, string userName, string[] roles)
        {
            this.Key = key;
            this.UserName = userName;
            this.Roles = roles;
        }

        public string[] Roles { get; }

        public string UserName { get; }

        public string Key { get; }
    }
}