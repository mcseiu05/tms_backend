using System;
using System.Collections.Generic;

namespace TMS.Common.Model
{
    public partial class Tasks
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AssignedTo { get; set; }
        public int? AssignedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public int? TaskStatus { get; set; }
        public int? Priority { get; set; }
    }
}
