using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class DisplayMedia : BaseEntity
    {
        public int Id { get; set; }
        public double Order { get; set; }
        public int MediaId { get; set; }
        public Media Media { get; set; }
    }
}
