using ApplicationCore.Entities;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ITicketService
    {
        Task<Ticket> CreateNewTicket(int idTask, int idPriority);
        Task<IEnumerable<Ticket>> GetTickets(StatusTicket statusTicket, int idOffice);
        Task<IReadOnlyList<string>> GetOfficesTask(int idTask);
        Task<Ticket> SetTicketInCalled(long idTicket, int idOffice, string idOperator);
        Task<Ticket> SetTicketInAssistance(long idTicket);
        Task<Ticket> SetTicketInProcessed(long idTicket);
        Task<Ticket> SetTicketInNotProcessed(long idTicket);
    }
}
