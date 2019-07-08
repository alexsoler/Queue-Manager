using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class SystemCustom
    {
        [Display(Name = "Logo")]
        public string Logo { get; set; }
        [Display(Name = "Logo mini")]
        public string LogoMin { get; set; }
        [Display(Name = "Nombre de la organización")]
        public string Nombre { get; set; }
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
    }
}
