using ClickUpIntegration.Models.ApiModels.Shared;
using ClickUpIntegration.Models.ApiModels.Spaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.ApiModels.Folders
{
    public class Folders
    {
        public Folders()
        {
            MyFolders = new List<Folder>();
        }
        [JsonProperty("folders")]
        public List<Folder> MyFolders { get; set; }
    }
    public class Folder
    {
        public Folder()
        {
            Statuses = new List<Status>();
            Lists = new List<FolderList>();
        }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("orderindex")]
        public string OrderIndex { get; set; }

        [JsonProperty("override_statuses")]
        public bool OverrideStatuses { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("space")]
        public FolderSpace Space { get; set; }

        [JsonProperty("task_count")]
        public string TaskCount { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("statuses")]
        public List<Status> Statuses { get; set; }

        [JsonProperty("lists")]
        public List<FolderList> Lists { get; set; }

        [JsonProperty("permission_level")]
        public string PermissionLevel { get; set; }
    }

    public class FolderSpace
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("access")]
        public bool Access { get; set; }
    }

    public class FolderList
    {
        public FolderList()
        {
            Statuses = new List<Status>();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("orderindex")]
        public string OrderIndex { get; set; }

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

        [JsonProperty("space")]
        public FolderSpace Space { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("override_statuses")]
        public bool OverriderStatuses { get; set; }

        [JsonProperty("statuses")]
        public List<Status> Statuses { get; set; }

        [JsonProperty("permission_level")]
        public string PermissionLevel { get; set; }

    }
}
