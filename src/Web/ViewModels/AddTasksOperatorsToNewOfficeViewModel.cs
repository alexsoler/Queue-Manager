using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class AddTasksOperatorsToNewOfficeViewModel
    {
        public OfficeViewModel Office { get; set; }
        public IEnumerable<TaskCheckboxViewModel> Tasks { get; set; }
        public IEnumerable<OperatorCheckBoxViewModel> Operators { get; set; }
    }
}
