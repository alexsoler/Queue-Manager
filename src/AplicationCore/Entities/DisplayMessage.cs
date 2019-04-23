using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class DisplayMessage : BaseEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
