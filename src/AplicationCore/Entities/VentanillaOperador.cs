using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class VentanillaOperador
    {
        public int VentanillaId { get; set; }
        public string ApplicationUserId { get; set; }
        public Ventanilla Ventanilla { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
