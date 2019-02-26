using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IOfficeViewModel
    {
        OfficeIndexViewModel GetOfficesPagination(int pageIndex, int itemsPage, string filteName = "");
        Task<OfficeEditViewModel> GetEditViewModel(int idOffice);
    }
}
