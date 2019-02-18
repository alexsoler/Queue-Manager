using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Office : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Prefix { get; set; }
        public List<OfficeTask> OfficeTasks { get; set; }
        public List<OfficeOperator> OfficeOperators { get; set; }
    }
}
