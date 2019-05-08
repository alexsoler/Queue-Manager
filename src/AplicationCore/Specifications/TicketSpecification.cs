using ApplicationCore.Entities;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public TicketSpecification(StatusTicket statusTicket, List<TaskEntity> tasks)
            : base( x => x.StatusId == (int)statusTicket && tasks.Contains(x.TaskEntity))
        {
            AddInclude(x => x.Priority);
            AddInclude(x => x.TaskEntity);
        }

        public TicketSpecification(long idTicket, bool includeOffice = false)
            : base(x => x.Id == idTicket)
        {
            AddInclude(x => x.Priority);
            AddInclude(x => x.TaskEntity);
            if(includeOffice)
                AddInclude(x => x.Office);
        }

        public TicketSpecification(int month, int year, StatusTicket status)
            : base(x => x.CreationDate.Month.Equals(month) && x.CreationDate.Year.Equals(year)
                    && x.StatusId.Equals((int)status))
        {

        }

        public TicketSpecification(StatusTicket status)
            : base(x => x.StatusId.Equals((int)status))
        {

        }
    }
}
