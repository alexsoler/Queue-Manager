using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.QueueManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Hubs.ParametersObject;

namespace Web.Hubs
{
    public class QueueHub : Hub<IQueueClient>
    {
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public QueueHub(ITicketService ticketService,
            IMapper mapper)
        {
            _ticketService = ticketService;
            _mapper = mapper;
        }

        public async Task CreateToken(CreateTicketParameter createTokenParameter)
        {
            var ticket = await _ticketService.CreateNewTicket(createTokenParameter.IdTarea, createTokenParameter.IdPrioridad);

            var ticketParameter = _mapper.Map<TicketParameter>(ticket);

            var idOffices = await _ticketService.GetOfficesTask(createTokenParameter.IdTarea);

            await Clients.Groups(idOffices).ReceiveToken(ticketParameter);
            await Clients.Caller.ReceiveToken(ticketParameter);
        }

        public async Task CallClient(long idTicket, int idOffice)
        {
            var operatorId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var ticket = await _ticketService.SetTicketInCalled(idTicket, idOffice, operatorId);

            var officesGroup = await _ticketService.GetOfficesTask(ticket.TaskEntityId);

            await Clients.Groups(officesGroup).RemoveTicketCalled(idTicket);
            await Clients.Caller.ToAttendTicket(idTicket);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).ConnectToOffice($"Se agrego el usuario al grupo de la oficina de id: {groupName}");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
