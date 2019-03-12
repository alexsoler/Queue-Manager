using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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

            await Clients.Caller.ReceiveToken(ticketParameter);
        }
    }
}
