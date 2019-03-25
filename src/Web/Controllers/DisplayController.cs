using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class DisplayController : Controller
    {
        private readonly IWritableOptions<DisplayTickets> _options;

        public DisplayController(IWritableOptions<DisplayTickets> options)
        {
            _options = options;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveState(string actualTicket, string actualOffice)
        {
            _options.Update(opt => {
                opt.TicketH3 = opt.TicketH2;
                opt.OfficeH3 = opt.OfficeH2;
                opt.TicketH2 = opt.TicketH1;
                opt.OfficeH2 = opt.OfficeH1;
                opt.TicketH1 = opt.ActualTicket;
                opt.OfficeH1 = opt.ActualOffice;
                opt.ActualTicket = actualTicket;
                opt.ActualOffice = actualOffice;
            });

            return Ok();
        }
    }
}