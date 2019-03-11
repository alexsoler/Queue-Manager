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
        public QueueHub()
        {

        }

        public async Task CreateToken(CreateTicketParameter createTokenParameter)
        {
            var ticketParameter = new TicketParameter();

            await Clients.All.ReceiveToken(ticketParameter);
        }
    }
}
