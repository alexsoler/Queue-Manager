using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class TicketService : ITicketService
    {
        private readonly IAsyncRepository<Ticket> _asyncRepository;
        private readonly IAsyncRepository<TaskEntity> _taskRepository;
        private readonly IAsyncRepository<OfficeTask> _officeTaskRepository;
        private readonly IAppLogger<TicketService> _logger;

        private static int numberTicket = 0;

        public TicketService(IAsyncRepository<Ticket> asyncRepository,
            IAsyncRepository<TaskEntity> taskRepository,
            IAsyncRepository<OfficeTask> officeTaskRepository,
            IAppLogger<TicketService> logger)
        {
            _asyncRepository = asyncRepository;
            _taskRepository = taskRepository;
            _officeTaskRepository = officeTaskRepository;
            _logger = logger;
        }

        public async Task<Ticket> CreateNewTicket(int idTask, int idPriority)
        {
            _logger.LogInformation("Creando un nuevo ticket");
            //int numberTicket = 0;

            //var specificationTicket = new TicketSpecification(StatusTicket.OnHold, OrderByDescending: true);
            //var lastTicket = await _asyncRepository.GetSingleBySpecAsync(specificationTicket);

            var task = await _taskRepository.GetByIdAsync(idTask);

 
                if (numberTicket >= 99)
                    numberTicket = 0;
            

            var newTicket = new Ticket
            {
                TaskEntityId = idTask,
                PriorityId = idPriority,
                StatusId = (int)StatusTicket.OnHold,
                NumberTicket = ++numberTicket,
                DisplayTokenName = string.Format("{0}{1:000}", task.Prefix, numberTicket),
                CreationDate = DateTime.Now
            };

            await _asyncRepository.AddAsync(newTicket);

            _logger.LogInformation($"Se creo el ticket {newTicket.DisplayTokenName} de id {newTicket.Id}");

            newTicket = await _asyncRepository.GetSingleBySpecAsync(new TicketSpecification(newTicket.Id));

            return newTicket;

        }

        public async Task<IEnumerable<Ticket>> GetTickets(StatusTicket statusTicket, int idOffice)
        {
            var officeTasks = await _officeTaskRepository.ListAsync(new OfficeWithTaskSpecification(idOffice, includeTask: true));

            var tasks = officeTasks.Select(x => x.Task).ToList();

            var specificationTicket = new TicketSpecification(statusTicket, tasks);

            return await _asyncRepository.ListAsync(specificationTicket);
        }

        public async Task<IReadOnlyList<string>> GetOfficesTask(int idTask)
        {
            var taskOffices = await _officeTaskRepository.ListAsync(new TaskWithOffices(idTask, includeOffice: true));

            return taskOffices.Select(x => x.Office.Id.ToString()).ToList();
        }

        /// <summary>
        /// Cambia el status del ticket a llamado y agrega la oficina donde se llamo asi como el operador
        /// </summary>
        /// <param name="idTicket">Id del ticket que se va a modificar</param>
        /// <param name="idOffice">Id de la oficina donde se llamo</param>
        /// <param name="idOperator">Id del operador que lo llamo</param>
        /// <returns>El ticket modificado</returns>
        public async Task<Ticket> SetTicketInCalled(long idTicket, int idOffice, string idOperator)
        {
            var ticket = await _asyncRepository.GetByIdAsync(idTicket);

            ticket.StatusId = (int)StatusTicket.Called;
            ticket.OfficeId = idOffice;
            ticket.ApplicationUserId = idOperator;

            await _asyncRepository.UpdateAsync(ticket);

            return ticket;
        }

        public async Task<Ticket> SetTicketInAssistance(long idTicket)
        {
            var ticket = await _asyncRepository.GetByIdAsync(idTicket);

            ticket.StatusId = (int)StatusTicket.InAssistance;
            ticket.StartAttentionDate = DateTime.Now;

            await _asyncRepository.UpdateAsync(ticket);

            return ticket;
        }

        public async Task<Ticket> SetTicketInProcessed(long idTicket)
        {
            var ticket = await _asyncRepository.GetByIdAsync(idTicket);

            ticket.StatusId = (int)StatusTicket.Processed;
            ticket.CompletionAttentionDate = DateTime.Now;

            await _asyncRepository.UpdateAsync(ticket);

            return ticket;
        }

        public async Task<Ticket> SetTicketInNotProcessed(long idTicket)
        {
            var ticket = await _asyncRepository.GetByIdAsync(idTicket);

            ticket.StatusId = (int)StatusTicket.NotProcessed;

            await _asyncRepository.UpdateAsync(ticket);

            return ticket;
        }
    }
}
