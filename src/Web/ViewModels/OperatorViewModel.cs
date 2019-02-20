using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class OperatorViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
