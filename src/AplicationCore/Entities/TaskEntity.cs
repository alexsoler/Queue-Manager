using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class TaskEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public List<OfficeTask> OfficesTasks { get; set; }
    }
}
