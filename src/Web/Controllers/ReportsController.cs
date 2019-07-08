using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = RolesStatic.Admin)]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAsyncRepository<Office> _officeRepository;
        private readonly IMapper _mapper;

        public ReportsController(IReportService reportService,
            UserManager<ApplicationUser> userManager,
            IAsyncRepository<Office> officeRepository,
            IMapper mapper)
        {
            _reportService = reportService;
            _userManager = userManager;
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Operators()
        {
            var operators = await _userManager.GetUsersInRoleAsync(RolesStatic.Operator);

            var operatorsvm = new SelectList(_mapper.Map<IEnumerable<OperatorViewModel>>(operators),
                                             "Id", "Name");

            return View(operatorsvm);
        }

        public async Task<IActionResult> ReportOperator(string idOperator, DateTime? initialDate, DateTime? endDate,
            Rotativa.AspNetCore.Options.Size size)
        {
            var reportVM = new OperatorReportViewModel();
            var userr = await _userManager.FindByIdAsync(idOperator);

            reportVM.NombreOperador = userr.Name;
            reportVM.InitialDate = initialDate;
            reportVM.EndDate = endDate;
            var tickets = await _reportService.GetTicketsForOperator(idOperator, initialDate, endDate);

            reportVM.tickets = _mapper.Map<IEnumerable<TicketReportViewModel>>(tickets);

            return new ViewAsPdf(reportVM)
            {
                PageSize = size,
                Cookies = Request.Cookies.ToDictionary(x => x.Key, x => x.Value)
            };
        }

        public IActionResult Periods()
        {
            return View();
        }

        public async Task<IActionResult> ReportPeriod(DateTime? initialDate, DateTime? endDate,
            Rotativa.AspNetCore.Options.Size size)
        {
            var reportVM = new PeriodReportViewModel();
            
            var tickets = await _reportService.GetTicketsForPeriod(initialDate, endDate);

            reportVM.InitialDate = initialDate;
            reportVM.EndDate = endDate;
            reportVM.tickets = _mapper.Map<IEnumerable<TicketReportViewModel>>(tickets);

            return new ViewAsPdf(reportVM)
            {
                PageSize = size,
                Cookies = Request.Cookies.ToDictionary(x => x.Key, x => x.Value)
            };
        }

        public async Task<IActionResult> Office()
        {
            var offices = await _officeRepository.ListAllAsync();

            var selectableOffices = new SelectList(_mapper.Map<IEnumerable<OfficeViewModel>>(offices), "Id", "Name");
            return View(selectableOffices);
        }

        public async Task<IActionResult> ReportOffice(int? idOffice, DateTime? initialDate, DateTime? endDate,
            Rotativa.AspNetCore.Options.Size size)
        {
            if (!idOffice.HasValue)
                return NotFound();

            var reportVM = new OfficeReportViewModel();
            var office = await _officeRepository.GetByIdAsync(idOffice.Value);

            var tickets = await _reportService.GetTicketsForOffice(idOffice.Value, initialDate, endDate);

            reportVM.Oficina = office.Name;
            reportVM.InitialDate = initialDate;
            reportVM.EndDate = endDate;
            reportVM.Tickets = _mapper.Map<IEnumerable<TicketReportViewModel>>(tickets);

            return new ViewAsPdf(reportVM)
            {
                PageSize = size,
                Cookies = Request.Cookies.ToDictionary(x => x.Key, x => x.Value)
            };
        }
    }
}