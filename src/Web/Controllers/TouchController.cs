using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.ViewModels;

namespace Web.Controllers
{
    public class TouchController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly IRepository<Priority> _repositoryPriority;
        private readonly ILogger<TouchController> _logger;

        public TouchController(ITaskService taskService,
            IMapper mapper,
            IRepository<Priority> repository,
            ILogger<TouchController> logger)
        {
            _taskService = taskService;
            _mapper = mapper;
            _repositoryPriority = repository;
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
    }
}