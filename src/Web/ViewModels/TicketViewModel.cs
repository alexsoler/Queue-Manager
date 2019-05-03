using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs.ParametersObject;
using Web.Models;

namespace Web.ViewModels
{
    public class TicketViewModel
    {

        public TicketParameter Ticket { get; set; }
        public TicketCustom Custom { get; set; }
    }
}
