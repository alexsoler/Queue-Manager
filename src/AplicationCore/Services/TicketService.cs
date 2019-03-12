using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class TicketService : ITicketService
    {
        private readonly IAsyncRepository<Ticket> _asyncRepository;
        private readonly IAsyncRepository<TaskEntity> _taskRepository;
        private readonly IAppLogger<TicketService> _logger;

        public TicketService(IAsyncRepository<Ticket> asyncRepository,
            IAsyncRepository<TaskEntity> taskRepository,
            IAppLogger<TicketService> logger)
        {
            _asyncRepository = asyncRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<Ticket> CreateNewTicket(int idTask, int idPriority)
        {
            _logger.LogInformation("Creando un nuevo ticket");
            int numberTicket = 0;

            var specificationTicket = new TicketSpecification(StatusTicket.OnHold);
            var lastTicket = await _asyncRepository.GetSingleBySpecAsync(specificationTicket);

            var task = await _taskRepository.GetByIdAsync(idTask);

            if (lastTicket != null)
            {
                if(lastTicket.NumberTicket < 999)
                    numberTicket = lastTicket.NumberTicket;
            }

            var newTicket = new Ticket
            {
                TaskEntityId = idTask,
                PriorityId = idPriority,
                StatusId = (int)StatusTicket.OnHold,
                NumberTicket = numberTicket + 1,
                DisplayTokenName = string.Format("{0}{1:000}", task.Prefix, numberTicket + 1),
                CreationDate = DateTime.Now
            };

            await _asyncRepository.AddAsync(newTicket);

            _logger.LogInformation($"Se creo el ticket {newTicket.DisplayTokenName} de id {newTicket.Id}");

            newTicket = await _asyncRepository.GetSingleBySpecAsync(new TicketSpecification(newTicket.Id));

            return newTicket;
            
        }
    }
}
