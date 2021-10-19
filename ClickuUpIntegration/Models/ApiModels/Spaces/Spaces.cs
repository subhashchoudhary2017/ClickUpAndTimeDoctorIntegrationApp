using ClickUpIntegration.Models.ApiModels.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.ApiModels.Spaces
{
    public class Spaces
    {
        public Spaces()
        {
            MySpaces = new List<Space>();
        }
        [JsonProperty("spaces")]
        public List<Space> MySpaces { get; set; }

    }
    public class Space
    {
        public Space()
        {
            Statuses = new List<Status>();
        }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("private")]
        public string Private { get; set; }

        [JsonProperty("statuses")]
        public List<Status> Statuses { get; set; }

        [JsonProperty("multiple_assignees")]
        public bool MultipleAssignees { get; set; }

        [JsonProperty("features")]
        public Features Features { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }
    }

    public class Features
    {
        [JsonProperty("due_dates")]
        public DueDates DueDates { get; set; }

        [JsonProperty("sprints")]
        public Sprits Sprits { get; set; }

        [JsonProperty("time_tracking")]
        public TimeTracking TimeTracking { get; set; }

        [JsonProperty("points")]
        public Points Points { get; set; }

        [JsonProperty("custom_items")]
        public CustomItems CustomItems { get; set; }

        [JsonProperty("priorities")]
        public Priority Priority { get; set; }

        [JsonProperty("tags")]
        public Tags Tags { get; set; }

        [JsonProperty("time_estimates")]
        public TimeEstimations TimeEstimations { get; set; }

        [JsonProperty("check_unresolved")]
        public CheckUnresolved CheckUnresolved { get; set; }

        [JsonProperty("zoom")]
        public Zoom Zoom { get; set; }

        [JsonProperty("milestones")]
        public MileStones MileStones { get; set; }

        [JsonProperty("custom_fields")]
        public CustomFields CustomFields { get; set; }

        [JsonProperty("remap_dependencies")]
        public RemapDependencies RemapDependencies { get; set; }

        [JsonProperty("dependency_warning")]
        public DependencyWarning DependencyWarning { get; set; }

        [JsonProperty("multiple_assignees")]
        public MultipleAssignees MultipleAssignees { get; set; }

        [JsonProperty("emails")]
        public Emails Emails { get; set; }
    }

    public class DueDates
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("start_date")]
        public bool StartDate { get; set; }

        [JsonProperty("remap_due_dates")]
        public bool RemapDueDates { get; set; }

        [JsonProperty("remap_closed_due_date")]
        public bool RemapClosedDueDate { get; set; }
    }
    public class Sprits
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class TimeTracking
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("harvest")]
        public bool Harvest { get; set; }

        [JsonProperty("rollup")]
        public bool RollUp { get; set; }
    }
    public class Points
    {
        [JsonProperty("")]
        public bool Enabled { get; set; }
    }
    public class CustomItems
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class Priority
    {
        public Priority()
        {
            Priorities = new List<Priorities>();
        }
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("priorities")]
        public List<Priorities> Priorities { get; set; }
    }
    public class Priorities
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("orderindex")]
        public string OrderIndex { get; set; }
    }
    public class Tags
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class TimeEstimations
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("rollup")]
        public bool RollUp { get; set; }

        [JsonProperty("per_assignee")]
        public bool PerAssignee { get; set; }
    }
    public class CheckUnresolved
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("subtasks")]
        public bool SubTasks { get; set; }

        [JsonProperty("checklists")]
        public string CheckLists { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }
    }

    public class Zoom
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class MileStones
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class CustomFields
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class RemapDependencies
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class DependencyWarning
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class MultipleAssignees
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
    public class Emails
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
