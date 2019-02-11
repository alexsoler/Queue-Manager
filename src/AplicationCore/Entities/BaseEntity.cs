using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
