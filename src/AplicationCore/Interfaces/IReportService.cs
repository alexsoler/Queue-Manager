using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<Ticket>> GetTicketsForOperator(string idOperator, DateTime? initialDate, DateTime? endDate);
        Task<IEnumerable<Ticket>> GetTicketsForPeriod(DateTime? initialDate, DateTime? endDate);
        Task<IEnumerable<Ticket>> GetTicketsForOffice(int idOffice, DateTime? initialDate, DateTime? endDate);
    }
}
