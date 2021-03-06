﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs.ParametersObject;

namespace Web.Hubs
{
    public interface IQueueClient
    {
        Task ReceiveToken(TicketParameter tokenParameter);
        Task ConnectToOffice(string message);
        Task RemoveTicketCalled(long id);
        Task ToAttendTicket(long id);
        Task CallDisplayTicket(TicketDisplayParameter ticket);
        Task CallBackDisplayTicket(TicketDisplayParameter ticket);
        Task Reload();
    }
}
