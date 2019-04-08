using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class DisplayMediaViewModel
    {
        public int Id { get; set; }
        public double Order { get; set; }
        public string Nombre { get; set; }
        public int IdMedia { get; set; }
        public string Tipo { get; set; }
        public string ContentType { get; set; }
    }
}
