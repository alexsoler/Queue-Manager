using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class VentanillaTarea
    {
        public int VentanillaId { get; set; }
        public int TareaId { get; set; }
        public Ventanilla Ventanilla { get; set; }
        public Tarea Tarea { get; set; }
    }
}
