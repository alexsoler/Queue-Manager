﻿using ApplicationCore.Entities;
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

        [Display(Name = "Prefijo")]
        [Required(ErrorMessage = "El campo {0} es rquerido")]
        public Prefix Prefix { get; set; }

        public DateTime CreationDate { get; set; }

        [Display(Name = "Tareas")]
        public List<OfficeTask> OfficeTasks { get; set; }
        [Display(Name = "Operadores")]
        public List<OfficeOperator> OfficeOperators { get; set; }
    }

    public enum Prefix
    {
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, Ñ, O, P, Q, R,
        S, T, U, V, W, X, Y, Z
    }
}
