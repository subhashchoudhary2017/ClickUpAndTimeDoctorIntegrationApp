using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.ApiModels.Shared
{
    public class Status
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderindex")]
        public int OrderIndex { get; set; }

        [JsonProperty("status")]
        public string FolderOrSpaceStatus { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
