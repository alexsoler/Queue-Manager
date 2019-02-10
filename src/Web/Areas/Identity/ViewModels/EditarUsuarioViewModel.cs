using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Identity.ViewModels
{
    public class EditarUsuarioViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener una longitud de {1} a {2} caracteres", MinimumLength = 4)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El {0} debe tener longitud de {1} a {2} caracteres.", MinimumLength = 4)]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "El formato del numero telefonico no es el correcto")]
        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "El {0} debe tener longitud de {1} a {2} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar")]
        [Compare("NewPassword", ErrorMessage = "La contraseña y confirmar contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Rol")]
        public List<RolesAsignadosData> Roles { get; set; }
    }

    public class RolesAsignadosData
    {
        public string Rol { get; set; }
        public bool Asignado { get; set; }
    }
}
