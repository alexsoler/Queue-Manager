using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class TouchCustom
    {
        [Display(Name = "Mostrar Logo")]
        public bool ShowLogo { get; set; }
        [Display(Name = "Logo")]
        public string PathImageLogo { get; set; }
        [Display(Name = "Mostrar titulo")]
        public bool ShowTitle { get; set; }
        [Display(Name = "Titulo")]
        public string Title { get; set; }
        [Display(Name = "Fondo de color solido")]
        public bool ShowBackgroundColor { get; set; }
        [Display(Name = "Color de fondo")]
        public string BackgroundColor { get; set; }
        [Display(Name = "Imagen de fondo")]
        public string BackgroundImage { get; set; }
        [Display(Name = "Color tareas")]
        public string ColorButtonTask { get; set; }
        [Display(Name = "Color prioridades")]
        public string ColorButtonPriority { get; set; }
        [Display(Name = "Fuente")]
        public string FontFamily { get; set; }
        [Display(Name = "Mensaje de notificación")]
        public string MensajeNotificacion { get; set; }
    }
}
