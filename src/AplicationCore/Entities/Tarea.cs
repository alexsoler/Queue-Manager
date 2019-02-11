using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class Tarea : BaseEntity
    {
        public string Nombre { get; set; }
        public List<VentanillaTarea> VentanillasTareas { get; set; }
    }
}
