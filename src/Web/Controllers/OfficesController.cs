using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class OfficesController : Controller
    {
        private readonly IOfficeService _officeService;
        private readonly IMapper _mapper;
        private readonly IOfficeViewModel _officeViewModel;
        private readonly IAppLogger<OfficesController> _logger;
        private readonly IAddTasksOperatorsToNewOfficeViewModel _addTasksOperatorsToOffice;

        public OfficesController(IOfficeService officeService,
            IOfficeViewModel officeViewModel,
            IAddTasksOperatorsToNewOfficeViewModel addTasksOperatorsToOffice,
            IAppLogger<OfficesController> logger,
            IMapper mapper)
        {
            _officeService = officeService;
            _mapper = mapper;
            _officeViewModel = officeViewModel;
            _logger = logger;
            _addTasksOperatorsToOffice = addTasksOperatorsToOffice;
        }

        public IActionResult Index(int? pag)
        {
            var itemsPage = 5;
            var officeModel = _officeViewModel.GetOfficesPagination(pag ?? 1, itemsPage);

            return View(officeModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Prefix")] OfficeViewModel officeViewModel)
        {
            if(ModelState.IsValid)
            {
                officeViewModel.CreationDate = DateTime.Now;
                var office = _mapper.Map<Office>(officeViewModel);
                office.Activo = true;
                await _officeService.AddOfficeAsync(office);
                return RedirectToAction(nameof(AddTasksAndOperators), new { office.Id });
            }

            return View(officeViewModel);
        }

        public async Task<IActionResult> AddTasksAndOperators(int id)
        {
            var vm =  await _addTasksOperatorsToOffice.GetViewModel(id);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskAndOperatorsSave([FromForm]OfficeViewModel officevm)
        {
            List<string> errores = new List<string>();

            if(officevm.Tasks != null)
            {
                var resultAddTasks = await _officeService.AddTasksAsync(
                    officevm.Tasks.Select(x => x.Id).ToArray(), officevm.Id);

                errores.AddRange(resultAddTasks.Errors);
            }
           
            if(officevm.Operators != null)
            {
                var resultAddOperators = await _officeService.AddOperatorsAsync(
                officevm.Operators.Select(x => x.Id).ToArray(), officevm.Id);

                errores.AddRange(resultAddOperators.Errors);
                
            }

            return Ok(errores);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> PartialViewEditAsync(int id)
        {
            var officevm = await _officeViewModel.GetEditViewModel(id);

            return PartialView("_EditarOficina", officevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OfficeViewModel officeViewModel,
            int[] tareasNuevas, int[] tareasEliminadas, string[] operadoresNuevos, string[] operadoresEliminados)
        {
            if(officeViewModel != null)
            {
                var office = _mapper.Map<Office>(officeViewModel);
                var result = await _officeService.EditOfficeAsync(office);

                if (result.Succeeded)
                    _logger.LogInformation($"Se edito la oficia con id {officeViewModel.Id}.");
                else
                    _logger.LogInformation($"No se pudo editar la oficina con id {officeViewModel.Id}.");
            }


            if(tareasNuevas.Length > 0)
                await _officeService.AddTasksAsync(tareasNuevas, officeViewModel.Id);
            if(tareasEliminadas.Length > 0)
            {
                foreach (var item in tareasEliminadas)
                {
                    await _officeService.RemoveTaskAsync(item, officeViewModel.Id);
                }
            }

            if (operadoresNuevos.Length > 0)
                await _officeService.AddOperatorsAsync(operadoresNuevos, officeViewModel.Id);
            if(operadoresEliminados.Length > 0)
            {
                foreach (var item in operadoresEliminados)
                {
                    await _officeService.RemoveOperatorAsync(item, officeViewModel.Id);
                }
            }

            return Json(officeViewModel);
        }

        [ActionName("TableOffices")]
        public IActionResult PartialViewTableOfficesAsync(int? pag, string view, string filterName = "")
        {
            var itemsPage = 5;
            var officeModel = _officeViewModel.GetOfficesPagination(pag ?? 1, itemsPage, filterName);

            ViewData["view"] = view;

            return PartialView("_TableAndPagination", officeModel);
        }

        [ActionName("Details")]
        public async Task<IActionResult> PartialViewDetailsAsync(int id)
        {
            var officevm = _mapper.Map<OfficeViewModel>(
                await _officeService.GetOfficeAsync(id));

            officevm.Tasks = _mapper.Map<IEnumerable<TaskEntity>, List<TaskViewModel>>(
                await _officeService.GetTasksAsync(id));

            officevm.Operators = _mapper.Map<IEnumerable<ApplicationUser>, List<OperatorViewModel>>(
                await _officeService.GetOperatorsAsync(id));

            return PartialView("_DetallesOficina", officevm);
        }

        [ActionName("Delete")]
        public async Task<IActionResult> PartialViewDelete(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var officevm = _mapper.Map<Office, OfficeViewModel>(await _officeService.GetOfficeAsync(id.Value));

            if (officevm == null)
                return NotFound($"No se encontro la oficina con id {id}.");

            return PartialView("_EliminarOficina", officevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var result = await _officeService.DeleteOfficeAsync(id.Value);

            if (!result.Succeeded)
                return BadRequest($"No se pudo desactivar la oficina con id {id}.");

            return Ok("Oficina desactivada con exito");
        }

        [ActionName("DeletePermanent")]
        public async Task<IActionResult> PartialViewDeletePermanent(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var officevm = _mapper.Map<Office, OfficeViewModel>(
                await _officeService.GetOfficeAsync(id.Value, ignoreQueryFilter: true));

            if (officevm == null)
                return NotFound($"No se encontro la oficina con id {id}.");

            return PartialView("_EliminarOficinaPermanente", officevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePermanent(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var result = await _officeService.DeleteOfficeAsync(id.Value, ignoreQueryFilter: true);

            if (!result.Succeeded)
                return BadRequest($"No se pudo eliminar la oficina con id {id}.");

            return Ok("Oficina eliminada con exito");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateOffice(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var office = await _officeService.GetOfficeAsync(id.Value, ignoreQueryFilter: true);

            if (!office.Activo)
                office.Activo = true;

            try
            {
                await _officeService.EditOfficeAsync(office);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return Ok();
        }

        public IActionResult EditarOficina()
        {
            return View();
        }

        public IActionResult EliminarOficina()
        {
            return View();
        }

        public IActionResult Search(string currentSearch, string typeResult)
        {
            IQueryable<Office> allOffices;
            if (typeResult == "edit")
                allOffices = _officeService.GetOfficesAsync().Result.AsQueryable();
            else
            {
                allOffices = _officeService.GetOfficesAsync(ignoreQueryFilter: true)
                    .Result.AsQueryable();
            }

            var offices = from o in allOffices
                          select o;

            if (!string.IsNullOrEmpty(currentSearch))
                offices = offices.Where(x => x.Name.Contains(
                    currentSearch, StringComparison.CurrentCultureIgnoreCase));

            var officesvm = _mapper.Map<IEnumerable<Office>, IEnumerable<OfficeViewModel>>(offices.ToList());

            ViewData["typeResult"] = typeResult;

            return PartialView("_ResultadoSearch", officesvm);
        }
    }
}