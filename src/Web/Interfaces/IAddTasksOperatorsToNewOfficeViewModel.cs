using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IAddTasksOperatorsToNewOfficeViewModel
    {
        Task<AddTasksOperatorsToNewOfficeViewModel> GetViewModel(int idOffice);
    }
}
