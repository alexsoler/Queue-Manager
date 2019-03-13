using ApplicationCore.Entities;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class TicketSpecification : BaseSpecification<Ticket>
    {
        public TicketSpecification(StatusTicket statusTicket, bool OrderByDescending = false)
            : base(x => x.StatusId == (int)statusTicket)
        {
            if (OrderByDescending)
                ApplyOrderByDescending(x => x.NumberTicket);
            else
                ApplyOrderBy(x => x.NumberTicket);

            AddInclude(x => x.Priority);
            AddInclude(x => x.TaskEntity);
        }

        public TicketSpecification(StatusTicket statusTicket, List<TaskEntity> tasks, bool OrderByDescending = false)
            : base( x => x.StatusId == (int)statusTicket && tasks.Contains(x.TaskEntity))
        {
            if (OrderByDescending)
                ApplyOrderByDescending(x => x.NumberTicket);
            else
                ApplyOrderBy(x => x.NumberTicket);

            AddInclude(x => x.Priority);
            AddInclude(x => x.TaskEntity);
        }

        public TicketSpecification(long idTicket)
            : base(x => x.Id == idTicket)
        {
            AddInclude(x => x.Priority);
            AddInclude(x => x.TaskEntity);
        }
    }
}
