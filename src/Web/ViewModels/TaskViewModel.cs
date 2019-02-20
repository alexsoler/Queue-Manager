using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Display(Name = "Fecha de creación")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
