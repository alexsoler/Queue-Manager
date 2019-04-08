using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class DisplayController : Controller
    {
        private readonly IWritableOptions<DisplayTickets> _options;
        private readonly IDisplayMediaService _displayMediaService;
        private readonly IMapper _mapper;

        public DisplayController(IWritableOptions<DisplayTickets> options,
            IDisplayMediaService displayMediaService,
            IMapper mapper)
        {
            _options = options;
            _displayMediaService = displayMediaService;
            _mapper = mapper;
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

        public async Task<IActionResult> GetPlayList()
        {
            var displayMedia = await _displayMediaService.ListAllAsync();
            var displayMediavm = _mapper.Map<IEnumerable<DisplayMediaViewModel>>(displayMedia);

            return Json(displayMediavm);
        }
    }
}