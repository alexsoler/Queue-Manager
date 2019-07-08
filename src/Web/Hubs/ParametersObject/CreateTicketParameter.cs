using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Hubs.ParametersObject
{
    public class CreateTicketParameter
    {
        public int IdTarea { get; set; }
        public int IdPrioridad { get; set; }
    }
}
