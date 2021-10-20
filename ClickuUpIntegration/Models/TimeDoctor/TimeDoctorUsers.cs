using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.TimeDoctor
{
    public class TimeDoctorUsers
    {
        [JsonProperty("data")]
        public List<Users> Users { get; set; }
    }

    public class Users
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
