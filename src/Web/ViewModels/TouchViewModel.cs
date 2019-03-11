using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TouchViewModel
    {
        public List<TaskViewModel> Tasks { get; set; }
        public List<Priority> Priorities { get; set; }
    }
}
