using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Hubs.ParametersObject
{
    public class TicketParameter
    {
        public int Id { get; set; }
        public string DisplayTokenName { get; set; }
        public int NumberTicket { get; set; }
        public string NameTask { get; set; }
        public string NamePriority { get; set; }
        public string CreationDate { get; set; }
    }
}
