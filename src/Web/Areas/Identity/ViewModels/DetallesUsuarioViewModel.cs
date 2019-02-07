using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Identity.ViewModels
{
    public class DetallesUsuarioViewModel
    {
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }
        public IList<string> Roles { get; set; }
    }
}
