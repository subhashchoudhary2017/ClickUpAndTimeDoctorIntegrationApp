using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models
{
    public class TokenReadDto
    {
        [JsonProperty("data")]
        public TokenDataDto Data { get; set; }
    }
    public class TokenDataDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("expiresAt")]
        public string ExpiresAt { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

    }

    public class Token
    {
        public string Result { get; set; }
    }
}
