using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class DisplayMessageViewModel
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Message { get; set; }
    }
}
