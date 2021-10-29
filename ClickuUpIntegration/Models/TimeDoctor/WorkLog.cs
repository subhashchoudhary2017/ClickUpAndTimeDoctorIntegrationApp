using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.TimeDoctor
{
    public class TimeDoctorWorkLog
    {
        public TimeDoctorWorkLog()
        {
            WorkLog = new List<List<WorkLog>>();
        }
        [JsonProperty("data")]
        public List<List<WorkLog>> WorkLog { get; set; }
    }

    public class WorkLog
    {
        [JsonProperty("taskId")]
        public string TaskId { get; set; }

        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("start")]
        public DateTime? Start { get; set; }

        [JsonProperty("time")]
        public double Time { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("taskName")]
        public string TaskName { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        public string UserName { get; set; }
        public string TotalTimeSpent { get; set; }
    }

    public class ProjectTask
    {
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string TaskName { get; set; }
        public string TaskId { get; set; }
        public string TotalHour { get; set; }
        public int Order { get; set; }

        public double TotalTimeSpentInProject { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
