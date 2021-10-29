using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.TimeDoctor
{
    public class WorkLogByUser
    {
        public WorkLogByUser()
        {
            UserData = new List<ProjectTask>();
        }
        public string UserName { get; set; }
        public int GroupByType { get; set; }
        public string TotalUserTime { get; set; }
        public List<ProjectTask> UserData { get; set; }
    }
}
