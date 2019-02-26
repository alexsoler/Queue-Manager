using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class OfficeOperator : BaseEntity
    {
        public int OfficeId { get; set; }
        public string ApplicationUserId { get; set; }
        public Office Office { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
