using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class DisplayStyle
    {
        [Display(Name = "Color primario")]
        public string ColorPrimario { get; set; }
        [Display(Name = "Color secundario")]
        public string ColorSecundario { get; set; }
        [Display(Name = "Color de fuente primario")]
        public string ColorFuentePrimario { get; set; }
        [Display(Name = "Color de fuente secundario")]
        public string ColorFuenteSecundario { get; set; }
        [Display(Name = "Fuente")]
        public string FontFamily { get; set; }
        [Display(Name = "Volumen de reproductor multimedia")]
        public float VolumenMultimedia { get; set; }
        [Display(Name = "Volumen de voz")]
        public float VolumenVoz { get; set; }
        [Display(Name = "Duración por mensaje (ms)")]
        public int DuracionMensajes { get; set; }
        [Display(Name = "Duración por imagen (ms)")]
        public int DuracionImagen { get; set; }
    }
}
