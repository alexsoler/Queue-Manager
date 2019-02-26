using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class OfficeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es rquerido")]
        [StringLength(30, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(200)]
        public string Description { get; set; }

        [Display(Name = "Creación")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Tareas")]
        public List<TaskViewModel> Tasks { get; set; }
        [Display(Name = "Operadores")]
        public List<OperatorViewModel> Operators { get; set; }
    }
}
