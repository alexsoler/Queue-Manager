using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class Office : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Prefix { get; set; }
        public List<OfficeTask> OfficesTasks { get; set; }
        public List<OfficeOperator> OfficesOperators { get; set; }
    }
}
