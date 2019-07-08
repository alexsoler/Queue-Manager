using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class OfficeReportViewModel
    {
        public string Oficina { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<TicketReportViewModel> Tickets { get; set; }
    }
}
