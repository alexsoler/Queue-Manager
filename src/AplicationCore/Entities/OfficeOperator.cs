using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class OfficeOperator : BaseEntity
    {
        public int OfficeId { get; set; }
        public string ApplicationUserId { get; set; }
        public Office Office { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
