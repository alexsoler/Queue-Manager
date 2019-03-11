using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs.ParametersObject;

namespace Web.Hubs
{
    public interface IQueueClient
    {
        Task ReceiveToken(TicketParameter tokenParameter);
    }
}
