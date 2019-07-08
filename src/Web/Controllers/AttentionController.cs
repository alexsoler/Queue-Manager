using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Hubs.ParametersObject;

namespace Web.Controllers
{
    [Authorize(Roles = RolesStatic.Operator)]
    public class AttentionController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IOperatorService _operatorService;
        private readonly IMapper _mapper;
        private readonly IAppLogger<AttentionController> _logger;

        public AttentionController(ITicketService ticketService,
            IOperatorService operatorService,
            IMapper mapper,
            IAppLogger<AttentionController> logger)
        {
            _ticketService = ticketService;
            _operatorService = operatorService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var idOperator = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ViewData["offices"] = await _operatorService.GetOffices(idOperator);
            return View();
        }

        public async Task<IActionResult> OnHoldTable(int? idOffice, [FromServices]IOfficeService officeService)
        {
            if (!idOffice.HasValue)
                return NotFound();

            var tasksOffice = await officeService.GetTasksAsync(idOffice.Value);

            ViewData["tasks"] = tasksOffice.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToList();

            var ticketsOnHold = await _ticketService.GetTickets(StatusTicket.OnHold, idOffice.Value);

            var ticketsOnHoldvm = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketParameter>>(ticketsOnHold);

            return PartialView("_TableTicketsOnHold", ticketsOnHoldvm);
        }

        [HttpPost]
        public async Task<IActionResult> StartAttention(long? id)
        {
            if (!id.HasValue)
                return Conflict("El parametro id llego nulo");

            var ticket = await _ticketService.SetTicketInAssistance(id.Value);

            if (ticket == null)
                return NotFound();

            _logger.LogInformation($"El ticket de id {id} se establecio en atención.");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> FinalizeAttention(long? id)
        {
            if (!id.HasValue)
                return Conflict("El parametro id llego nulo");

            var ticket = await _ticketService.SetTicketInProcessed(id.Value);

            if(ticket == null)
                return NotFound();

            _logger.LogInformation($"El ticket de id {id} se establecio en procesado.");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> NotAttention(long? id)
        {
            if (!id.HasValue)
                return Conflict("El parametro id llego nulo");

            var ticket = await _ticketService.SetTicketInNotProcessed(id.Value);

            if (ticket == null)
                return NotFound();

            _logger.LogInformation($"El ticket de id {id} se establecio en procesado.");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ClearAll(long[] idTickets)
        {
            foreach (var id in idTickets)
            {
                await _ticketService.SetTicketInNotProcessed(id);
            }

            return Ok();
        }
    }
}