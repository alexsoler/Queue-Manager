using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
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
    public class OfficeViewModelService : IOfficeViewModel
    {
        private readonly IOfficeService _officeService;
        private readonly IRepository<Office> _officeRepository;
        private readonly IRepository<TaskEntity> _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public OfficeViewModelService(IOfficeService officeService,
            IRepository<Office> officeRepository,
            IRepository<TaskEntity> taskRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _officeService = officeService;
            _officeRepository = officeRepository;
            _taskRepository = taskRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<OfficeEditViewModel> GetEditViewModel(int idOffice)
        {
            var vm = new OfficeEditViewModel();

            var office = await _officeService.GetOfficeAsync(idOffice);

            vm.Office = _mapper.Map<OfficeViewModel>(office);

            vm.Tasks = new List<TaskCheckboxViewModel>();
            vm.Operators = new List<OperatorCheckBoxViewModel>();

            foreach (var item in _taskRepository.ListAll())
            {
                if (await _officeService.HasTask(office.Id, item.Id))
                    vm.Tasks.Add(new TaskCheckboxViewModel { Id = item.Id, Name = item.Name, Asignado = true });
                else
                    vm.Tasks.Add(new TaskCheckboxViewModel { Id = item.Id, Name = item.Name, Asignado = false });
            }

            var operadores = await _userManager.GetUsersInRoleAsync("Agente de Atención");
            foreach (var item in operadores)
            {
                if (await _officeService.HasOperator(office.Id, item.Id))
                    vm.Operators.Add(new OperatorCheckBoxViewModel { Id = item.Id, Name = item.Name, Asignado = true });
                else
                    vm.Operators.Add(new OperatorCheckBoxViewModel { Id = item.Id, Name = item.Name, Asignado = false });
            }

            return vm;
        }

        public OfficeIndexViewModel GetOfficesPagination(int pageIndex, int itemsPage, string filterName = "")
        {
            var filterPaginatedSpecification =
                new OfficesFilterPaginatedSpecification(itemsPage * (pageIndex - 1), itemsPage, filterName);

            var officesOnPage = _mapper.Map<IEnumerable<Office>, IEnumerable<OfficeViewModel>>(
                _officeRepository.List(filterPaginatedSpecification));
            var totalOffices = _officeRepository.Count(new OfficeSpecification(filterName));

            var vm = new OfficeIndexViewModel()
            {
                OfficesList = officesOnPage,
                PaginationInfo = new PaginationInfoViewModel()
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = officesOnPage.Count(),
                    TotalItems = totalOffices,
                    TotalPages = int.Parse(Math.Ceiling((decimal)totalOffices / itemsPage).ToString())
                }
            };

            vm.PaginationInfo.Controller = "Offices";
            vm.PaginationInfo.Action = "Index";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages) ? "disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 1) ? "disabled" : "";

            return vm;
        }
    }
}
