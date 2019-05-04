using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Comentario"), StringLength(500)]
        public string Message { get; set; }
        [Display(Name = "¿Visto?")]
        public bool IsView { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
