using ClickUpIntegration.Models.ApiModels.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.ApiModels.Tasks
{
    public class ListTasks
    {
        public ListTasks()
        {
            MyTasks = new List<ListTask>();
        }

        [JsonProperty("tasks")]
        public List<ListTask> MyTasks { get; set; }
    }

    public class ListTasksByType
    {
        public ListTasksByType()
        {
            Tasks = new List<ListTask>();
        }

        [JsonProperty("tasks")]
        public string Type { get; set; }
        public List<ListTask> Tasks { get; set; }
    }

    public class ListTask
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("custom_id")]
        public string CustomId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("text_content")]
        public string TextContent { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("orderindex")]
        public string OrderIndex { get; set; }

        [JsonProperty("date_created")]
        public string DateCreated { get; set; }

        [JsonProperty("date_updated")]
        public string DateUpdated { get; set; }

        [JsonProperty("date_closed")]
        public string DateClosed { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("creator")]
        public Creator Creator { get; set; }

        [JsonProperty("assignees")]
        public List<Assignee> Assignees { get; set; }

        //[JsonProperty("watchers")]
        //public List<object> Watchers { get; set; }

        [JsonProperty("checklists")]
        public List<string> Checklists { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        //[JsonProperty("priority")]
        //public object Priority { get; set; }

        [JsonProperty("due_date")]
        public string DueDate { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("points")]
        public string Points { get; set; }

        [JsonProperty("time_estimate")]
        public string TimeEstimate { get; set; }

        //[JsonProperty("custom_fields")]
        //public List<object> CustomFields { get; set; }

        [JsonProperty("dependencies")]
        public List<string> Dependencies { get; set; }

        [JsonProperty("linked_tasks")]
        public List<string> LinkedTasks { get; set; }

        [JsonProperty("team_id")]
        public string TeamId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }


        [JsonProperty("permission_level")]
        public string PermissionLevel { get; set; }

        [JsonProperty("list")]
        public TaskList List { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("folder")]
        public TaskFolder Folder { get; set; }

        [JsonProperty("space")]
        public TaskSpace Space { get; set; }
    }

    public class CustomFields
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public TypeConfig TypeConfig { get; set; }
        public string DateCreated { get; set; }
        public bool HideFromGuests { get; set; }
        public string Value { get; set; }
        public bool? Required { get; set; }
    }

    public class TypeConfig
    {
        public bool IncludeGuests { get; set; }
        public bool IncludeTeamMembers { get; set; }
    }

    public class Creator
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }
    }

    public class Assignee
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("initials")]
        public string Initials { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("profilePicture")]
        public object ProfilePicture { get; set; }
    }

    public class TaskList
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("access")]
        public bool Access { get; set; }
    }

    public class Project
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

    public class TaskFolder
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

    public class TaskSpace
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
