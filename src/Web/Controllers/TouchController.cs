using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Web.Hubs.ParametersObject;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = RolesStatic.TouchScreen)]
    public class TouchController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly IRepository<Priority> _repositoryPriority;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<TouchController> _logger;

        public TouchController(ITaskService taskService,
            IMapper mapper,
            IRepository<Priority> repository,
            SignInManager<ApplicationUser> signInManager,
            ILogger<TouchController> logger)
        {
            _taskService = taskService;
            _mapper = mapper;
            _repositoryPriority = repository;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new TouchViewModel();

            var tasks = await _taskService.GetAllAsync();

            vm.Tasks = _mapper.Map<IEnumerable<TaskEntity>, List<TaskViewModel>>(tasks);
            vm.Priorities = _repositoryPriority.ListAll().ToList();

            return View(vm);
        }

        public async Task<IActionResult> LogOut(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                var user = await _signInManager.UserManager.FindByNameAsync(userName);

                if (await _signInManager.UserManager.CheckPasswordAsync(user, password))
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Ticket([FromServices]IOptionsMonitor<TicketCustom> options, TicketParameter ticketParameter, TicketCustom ticketCustom)
        {
            if (ticketCustom.PageWidth == 0)
                ticketCustom = options.CurrentValue;

            var ticketvm = new TicketViewModel()
            {
                Ticket = ticketParameter,
                Custom = ticketCustom
            };

            return new ViewAsPdf(ticketvm)
            {
                PageWidth = ticketCustom.PageWidth,
                PageHeight = ticketCustom.PageHeight,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(5, 5, 5, 5),
                IsGrayScale = true,
                Cookies = Request.Cookies.ToDictionary(x => x.Key, x => x.Value)
            };
        }
    }
}