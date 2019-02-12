using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class TaskEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<OfficeTask> OfficesTasks { get; set; }
    }
}
