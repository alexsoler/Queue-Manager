using ApplicationCore.Entities;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class TicketSpecification : BaseSpecification<Ticket>
    {
        public TicketSpecification(StatusTicket statusTicket)
            : base(x => x.StatusId == (int)statusTicket)
        {
            ApplyOrderByDescending(x => x.NumberTicket);
        }

        public TicketSpecification(long idTicket)
            : base(x => x.Id == idTicket)
        {
            AddInclude(x => x.Priority);
            AddInclude(x => x.TaskEntity);
        }
    }
}
