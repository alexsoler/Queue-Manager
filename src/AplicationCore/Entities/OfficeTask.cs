using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class OfficeTask : BaseEntity
    {
        public int OfficeId { get; set; }
        public int TaskId { get; set; }
        public Office Office { get; set; }
        public TaskEntity Task { get; set; }
    }
}
