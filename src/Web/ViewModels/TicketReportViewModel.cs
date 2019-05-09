using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TicketReportViewModel
    {
        public string DisplayTokenName { get; set; }
        public string NameTask { get; set; }
        public string NamePriority { get; set; }
        public string Estado { get; set; }
        public string CreationDate { get; set; }
        public string Duracion { get; set; }
    }
}
