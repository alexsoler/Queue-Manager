using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class MediaViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tipo de Archivo")]
        public string Tipo { get; set; }
        public bool Used { get; set; }
        [Display(Name = "Nombre del archivo")]
        public string Name { get; set; }
        public byte[] File { get; set; }
        [Display(Name = "Fecha de Creación")]
        public DateTime CreationDate { get; set; }
    }
}
