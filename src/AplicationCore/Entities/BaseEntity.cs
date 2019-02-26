using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class BaseEntity
    {
        public bool Activo { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
