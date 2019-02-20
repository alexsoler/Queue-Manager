using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class OfficeIndexViewModel
    {
        public IEnumerable<OfficeViewModel> OfficesList { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
    }
}
