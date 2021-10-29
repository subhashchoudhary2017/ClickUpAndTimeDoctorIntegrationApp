using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.TimeDoctor
{
    public class AuthenticateResultModel
    {
        [JsonProperty("data")]
        public TokenResult Data { get; set; }
    }

    public class TokenResult
    {
        [JsonProperty("id")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("expiresAt")]
        public string ExpireAt { get; set; }

        [JsonProperty("companies")]
        public List<Company> Companies { get; set; }
    }

    public class Company
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
