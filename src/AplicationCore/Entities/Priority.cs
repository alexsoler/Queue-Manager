using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Priority : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
