using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class PeriodReportViewModel
    {
        public DateTime? InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<TicketReportViewModel> tickets { get; set; }
    }
}
