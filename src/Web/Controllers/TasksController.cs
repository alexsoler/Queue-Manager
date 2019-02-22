using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ITaskIndexViewModel _taskIndexViewModel;
        private readonly IMapper _mapper;

        public TasksController(ITaskService taskService,
            ITaskIndexViewModel taskIndexViewModel,
            IMapper mapper)
        {
            _taskService = taskService;
            _taskIndexViewModel = taskIndexViewModel;
            _mapper = mapper;
        }

        public IActionResult Index(int? pag)
        {
            var itemsPage = 5;
            var taskModel = _taskIndexViewModel.GetTasksPagination(pag ?? 1, itemsPage);
            return View(taskModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]TaskViewModel newTask)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (newTask == null)
                return NotFound();

            var task = _mapper.Map<TaskEntity>(newTask);
            task.CreationDate = DateTime.Now;

            await _taskService.AddTaskAsync(task);

            return Ok(task);
        }

        [ActionName("TableTasks")]
        public IActionResult PartialViewTableTasks(int? pag)
        {
            var itemsPage = 5;
            var taskModel = _taskIndexViewModel.GetTasksPagination(pag ?? 1, itemsPage);

            return PartialView("_TableAndPagination", taskModel);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> PartialViewEditAsync(int? Id)
        {
            if (Id == null)
                return NotFound();

            var task = await _taskService.GetTaskAsync(Id.Value);

            var taskvm = _mapper.Map<TaskViewModel>(task);

            return PartialView("_EditarTarea", taskvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm]TaskViewModel taskViewModel)
        {
            if (taskViewModel == null)
                return Conflict();

            var task = _mapper.Map<TaskEntity>(taskViewModel);

            var result = await _taskService.EditTaskAsync(task);

            if (!result.Succeeded)
                return BadRequest("No se pudo editar la tarea");

            return Ok("Se edito la tarea");
        }
    }
}