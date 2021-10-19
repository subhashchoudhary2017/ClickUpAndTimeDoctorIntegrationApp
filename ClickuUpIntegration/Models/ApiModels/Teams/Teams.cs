using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.ApiModels
{
    public class Teams
    {
        public Teams()
        {
            MyTeams = new List<Team>();
        }
        [JsonProperty("teams")]
        public List<Team> MyTeams { get; set; }
    }
    public class Team
    {
        public Team()
        {
            Members = new List<Member>();
        }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }

    }
    public class Member
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("invited_by")]
        public InvitedBy InvitedBy { get; set; }
    }
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("initials")]
        public string Initials { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("custom_role")]
        public string Custom_Role { get; set; }

        [JsonProperty("last_active")]
        public string Last_Active { get; set; }

        [JsonProperty("date_joined")]
        public string Date_Joined { get; set; }

        [JsonProperty("date_invited")]
        public string Date_Invited { get; set; }

    }
    public class InvitedBy
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("initials")]
        public string Initials { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }
    }
}
