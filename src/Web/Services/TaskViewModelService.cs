using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class TaskViewModelService : ITaskIndexViewModel
    {
        private readonly IRepository<TaskEntity> _taskRepository;
        private readonly IMapper _mapper;

        public TaskViewModelService(IRepository<TaskEntity> taskRepository,
                IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public TaskIndexViewModel GetTasksPagination(int pageIndex, int itemsPage)
        {
            var filterPaginatedSpecification =
                new TasksFilterPaginatedSpecification(itemsPage * (pageIndex - 1), itemsPage);

            var tasksOnPage = _mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskViewModel>>(
                _taskRepository.List(filterPaginatedSpecification));
            var totalTasks = _taskRepository.Count(new TaskSpecification());

            var vm = new TaskIndexViewModel
            {
                TasksList = tasksOnPage,
                PaginationInfo = new PaginationInfoViewModel
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = tasksOnPage.Count(),
                    TotalItems = totalTasks,
                    TotalPages = int.Parse(Math.Ceiling((decimal)totalTasks / itemsPage).ToString())
                }
            };

            vm.PaginationInfo.Controller = "Tasks";
            vm.PaginationInfo.Action = "Index";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages) ? "disabled" : "" ;
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 1) ? "disabled" : "";

            return vm;
        }
    }
}
