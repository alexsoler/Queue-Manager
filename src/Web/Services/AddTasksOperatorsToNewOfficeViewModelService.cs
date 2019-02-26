using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class AddTasksOperatorsToNewOfficeViewModelService : IAddTasksOperatorsToNewOfficeViewModel
    {
        private readonly ITaskService _taskService;
        private readonly IOfficeService _officeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AddTasksOperatorsToNewOfficeViewModelService(ITaskService taskService,
            IOfficeService officeService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _taskService = taskService;
            _officeService = officeService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AddTasksOperatorsToNewOfficeViewModel> GetViewModel(int idOffice)
        {
            var vm = new AddTasksOperatorsToNewOfficeViewModel();

            var office = await _officeService.GetOfficeAsync(idOffice);
            vm.Office = _mapper.Map<OfficeViewModel>(office);

            var tasks = await _taskService.GetAllAsync();
            vm.Tasks = _mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskCheckboxViewModel>>(tasks);

            var operators = await _userManager.GetUsersInRoleAsync("Agente de Atención");
            vm.Operators = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<OperatorCheckBoxViewModel>>(operators);

            return vm;
        }
    }
}
