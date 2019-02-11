using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Entities
{
    public class Ventanilla : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public List<VentanillaTarea> VentanillasTareas { get; set; }
        public List<VentanillaOperador> VentanillasOperadores { get; set; }
    }
}
