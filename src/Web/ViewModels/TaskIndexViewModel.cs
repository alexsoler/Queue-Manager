using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TaskIndexViewModel
    {
        public IEnumerable<TaskViewModel> TasksList { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
    }
}
