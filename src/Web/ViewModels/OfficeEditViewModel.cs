using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class OfficeEditViewModel
    {
        public OfficeViewModel Office { get; set; }
        public List<TaskCheckboxViewModel> Tasks { get; set; }
        public List<OperatorCheckBoxViewModel> Operators { get; set; }
    }
}
