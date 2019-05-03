using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class TicketCustom
    {
        [Display(Name = "Mostrar logo")]
        public bool ShowLogo { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Display(Name = "Mostrar titulo")]
        public bool ShowTitle { get; set; }
        [Display(Name = "Titulo")]
        public string Title { get; set; }
        [Display(Name = "Tamaño (mm)")]
        public int SizeTitle { get; set; }
        [Display(Name = "Estilo")]
        public string FontStyleTitle { get; set; }
        [Display(Name = "Grosor")]
        public string FontWeightTitle { get; set; }

        [Display(Name = "Mostrar ticket")]
        public bool ShowTicket { get; set; }
        [Display(Name = "Tamaño (mm)")]
        public int SizeDisplayTicket { get; set; }
        [Display(Name = "Estilo")]
        public string FontStyleDisplayTicket { get; set; }
        [Display(Name = "Grosor")]
        public string FontWeightDisplayTicket { get; set; }

        [Display(Name = "Mostrar tarea")]
        public bool ShowTask { get; set; }
        [Display(Name = "Tamaño (mm)")]
        public int SizeTask { get; set; }
        [Display(Name = "Estilo")]
        public string FontStyleTask { get; set; }
        [Display(Name = "Grosor")]
        public string FontWeightTask { get; set; }

        [Display(Name = "Mostrar prioridad")]
        public bool ShowPriority { get; set; }
        [Display(Name = "Tamaño (mm)")]
        public int SizePriority { get; set; }
        [Display(Name = "Estilo")]
        public string FontStylePriority { get; set; }
        [Display(Name = "Grosor")]
        public string FontWeightPriority { get; set; }

        [Display(Name = "Mostrar fecha")]
        public bool ShowDate { get; set; }
        [Display(Name = "Tamaño (mm)")]
        public int SizeDate { get; set; }
        [Display(Name = "Estilo")]
        public string FontStyleDate { get; set; }
        [Display(Name = "Grosor")]
        public string FontWeightDate { get; set; }

        [Display(Name = "Mostrar telefono")]
        public bool ShowTelefono { get; set; }
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }
        [Display(Name = "Tamaño (mm)")]
        public int SizeTelefono { get; set; }
        [Display(Name = "Estilo")]
        public string FontStyleTelefono { get; set; }
        [Display(Name = "Grosor")]
        public string FontWeightTelefono { get; set; }

        [Display(Name = "Fuente")]
        public string FontFamily { get; set; }

        [Display(Name = "Ancho de pagina")]
        public double PageWidth { get; set; }
        [Display(Name = "Altura de pagina")]
        public double PageHeight { get; set; }
    }
}
