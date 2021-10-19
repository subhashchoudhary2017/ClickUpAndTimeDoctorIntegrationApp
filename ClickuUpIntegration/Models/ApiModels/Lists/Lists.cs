using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.ApiModels.Lists
{
    public class Lists
    {
        public Lists()
        {
            MyList = new List<List>();
        }
        [JsonProperty("lists")]
        public List<List> MyList { get; set; }
    }

    public class List
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("orderindex")]
        public int OrderIndex { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("assignee")]
        public string Assignee { get; set; }

        [JsonProperty("task_count")]
        public string TaskCount { get; set; }

        [JsonProperty("due_date")]
        public string DueDate { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("folder")]
        public ListFolder Folder { get; set; }

        [JsonProperty("space")]
        public ListSpace Space { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("override_statuses")]
        public bool OverrideStatuses { get; set; }

        [JsonProperty("permission_level")]
        public string PermissionLevel { get; set; }
    }

    public class ListFolder
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("access")]
        public bool Access { get; set; }
    }
    public class ListSpace
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }
    }
}
