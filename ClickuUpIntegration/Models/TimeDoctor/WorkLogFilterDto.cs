using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Models.TimeDoctor
{
    public class WorkLogFilterDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string UserId { get; set; }
        public string ProjectName { get; set; }
        public string CompanyId { get; set; }
        public int GroupByType { get; set; }
    }
}
