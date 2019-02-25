using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IAsyncRepository<Office> _officeRepository;
        private readonly IAsyncRepository<OfficeOperator> _officesOperatorsRepository;
        private readonly IAsyncRepository<OfficeTask> _officesTasksRepository;
        private readonly IAppLogger<OfficeService> _logger;

        public OfficeService(IAsyncRepository<Office> officeRepository,
            IAsyncRepository<OfficeOperator> officesOperatorsRepository,
            IAsyncRepository<OfficeTask> officesTasksRepository,
            IAppLogger<OfficeService> logger)
        {
            _officeRepository = officeRepository;
            _officesOperatorsRepository = officesOperatorsRepository;
            _officesTasksRepository = officesTasksRepository;
            _logger = logger;
        }

        public async Task<Office> AddOfficeAsync(Office office)
        {
            _logger.LogInformation($"Guardando nueva oficina con id { office.Id }.");
            return await _officeRepository.AddAsync(office);
        }

        public async Task<OperationResult> AddOperatorAsync(string IdOperator, int idOffice)
        {
            _logger.LogInformation($"Agregando un operador con id {IdOperator} a la oficina con id {idOffice}.");
            var officeHasOperator = await _officesOperatorsRepository.ExistAsync(
                new OfficeWithOperatorsSpecification(IdOperator, idOffice, ignoreQueryFilter: true));

            if (officeHasOperator)
            {
                var activateOperator = await _officesOperatorsRepository.GetSingleBySpecAsync(
                        new OfficeWithOperatorsSpecification(IdOperator, idOffice, ignoreQueryFilter: true));
                if (!activateOperator.Activo)
                {
                    activateOperator.Activo = true;
                    await _officesOperatorsRepository.UpdateAsync(activateOperator);
                    _logger.LogInformation($"Se volvio a activar el operador {IdOperator} en la oficina {idOffice}");

                    return new OperationResult { Succeeded = true };
                }
                else
                {
                    var operationResult = new OperationResult();
                    operationResult.Succeeded = false;
                    var error = $"La oficina ya tiene al operador con id {IdOperator}.";
                    _logger.LogInformation(error);
                    operationResult.Errors.Add(error);

                    return operationResult;
                }
            }

            var officeOperator = new OfficeOperator
            {
                ApplicationUserId = IdOperator,
                OfficeId = idOffice,
                CreationDate = DateTime.Now,
                Activo = true
            };

            await _officesOperatorsRepository.AddAsync(officeOperator);

            return new OperationResult { Succeeded = true };
        }

        public async Task<OperationResult> AddOperatorsAsync(string[] IdOperators, int idOffice)
        {
            _logger.LogInformation($"Agregando {IdOperators.Length} operadores a la oficina con id {idOffice}.");
            var officeOperators = new List<OfficeOperator>();
            var operationResult = new OperationResult();

            foreach (var id in IdOperators)
            {
                var officeHasOperator = await _officesOperatorsRepository.ExistAsync(
                    new OfficeWithOperatorsSpecification(id, idOffice, ignoreQueryFilter: true));

                if (!officeHasOperator)
                {
                    officeOperators.Add(new OfficeOperator
                    {
                        ApplicationUserId = id,
                        OfficeId = idOffice,
                        CreationDate = DateTime.Now,
                        Activo = true
                    });
                }
                else
                {
                    var activateOperator = await _officesOperatorsRepository.GetSingleBySpecAsync(
                        new OfficeWithOperatorsSpecification(id, idOffice, ignoreQueryFilter: true));
                    if(!activateOperator.Activo)
                    {
                        activateOperator.Activo = true;
                        await _officesOperatorsRepository.UpdateAsync(activateOperator);
                        _logger.LogInformation($"Se volvio a activar el operador {id} en la oficina {idOffice}");
                        operationResult.Succeeded = true;
                    }
                    else
                    {
                        var error = $"La oficina con id {idOffice} ya tiene al operador con id { id }.";
                        _logger.LogInformation(error);
                        operationResult.Errors.Add(error);
                        operationResult.Succeeded = false;
                    }
                }
            }

            if (officeOperators.Count > 0)
            {
                await _officesOperatorsRepository.AddRangeAsync(officeOperators);
                operationResult.Succeeded = true;
            }

            return operationResult;
        }

        public async Task<OperationResult> AddTaskAsync(int idTask, int idOffice)
        {
            _logger.LogInformation($"Agregando una tarea con id {idTask} a la oficina con id {idOffice}.");
            var operationResult = new OperationResult();
            var officeHasTask = await _officesTasksRepository.ExistAsync(
                new OfficeWithTaskSpecification(idTask, idOffice, true));

            if(officeHasTask)
            {
                var activateTask = await _officesTasksRepository.GetSingleBySpecAsync(
                    new OfficeWithTaskSpecification(idTask, idOffice, true));
                if (!activateTask.Activo)
                {
                    activateTask.Activo = true;
                    await _officesTasksRepository.UpdateAsync(activateTask);
                    _logger.LogInformation($"Se volvio a activar la tarea con id {idTask} en la oficina con id {idOffice}");
                    operationResult.Succeeded = true;
                    return operationResult;
                }
                else
                {
                    operationResult.Succeeded = false;
                    var error = $"La oficina ya tiene la tarea con id {idTask}.";
                    _logger.LogInformation(error);
                    operationResult.Errors.Add(error);

                    return operationResult;
                }
            }

            var officeTask = new OfficeTask
            {
                OfficeId = idOffice,
                TaskId = idTask,
                CreationDate = DateTime.Now,
                Activo = true
            };

            await _officesTasksRepository.AddAsync(officeTask);
            operationResult.Succeeded = true;

            return operationResult;

        }

        public async Task<OperationResult> AddTasksAsync(int[] idTasks, int idOffice)
        {
            _logger.LogInformation($"Agregando {idTasks.Length} operadores a la oficina con id {idOffice}.");
            var officeTask = new List<OfficeTask>();
            var operationResult = new OperationResult();

            foreach (var taskid in idTasks)
            {
                var officeHasTask = await _officesTasksRepository.ExistAsync(
                    new OfficeWithTaskSpecification(taskid, idOffice, true));

                if(!officeHasTask)
                {
                    officeTask.Add(new OfficeTask
                    {
                        TaskId = taskid,
                        OfficeId = idOffice,
                        CreationDate = DateTime.Now,
                        Activo = true
                    });
                }
                else
                {
                    var activateTask = await _officesTasksRepository.GetSingleBySpecAsync(
                        new OfficeWithTaskSpecification(taskid, idOffice, true));

                    if (!activateTask.Activo)
                    {
                        activateTask.Activo = true;
                        await _officesTasksRepository.UpdateAsync(activateTask);
                        _logger.LogInformation($"Se volvio a activar la tarea con id {taskid} en la oficina on id {idOffice}");
                        operationResult.Succeeded = true;
                    }
                    else
                    {
                        var error = $"La oficina ya tiene la tarea con id {taskid}.";
                        _logger.LogInformation(error);
                        operationResult.Errors.Add(error);
                        operationResult.Succeeded = false;
                    }
                }
            }

            if (officeTask.Count > 0)
            {
                await _officesTasksRepository.AddRangeAsync(officeTask);
                operationResult.Succeeded = true;
            }

            return operationResult;
        }

        public async Task<OperationResult> DeleteOfficeAsync(int id)
        {
            _logger.LogInformation($"Eliminando oficina con id {id}.");
            var office = await _officeRepository.GetByIdAsync(id);

            if(office == null)
            {
                var operationResult = new OperationResult { Succeeded = false };
                var error = $"La oficina con id {id} ya se encontraba eliminada.";
                _logger.LogInformation(error);
                operationResult.Errors.Add(error);

                return operationResult;
            }

            await _officeRepository.DeleteAsync(office);
            return new OperationResult { Succeeded = true };
        }

        public async Task<OperationResult> EditOfficeAsync(Office office)
        {
            await _officeRepository.UpdateAsync(office);
            return new OperationResult { Succeeded = true };
        }

        public async Task<Office> GetOfficeAsync(int id)
        {
            return await _officeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Office>> GetOfficesAsync(OfficeSpecification officeSpecification = null)
        {
            if(officeSpecification == null)
                return await _officeRepository.ListAllAsync();

            return await _officeRepository.ListAsync(officeSpecification);
        }

        public async Task<int> GetOfficesCountAsync(int id)
        {
            return await _officeRepository.CountAsync(
                new OfficeSpecification(id));
        }

        public async Task<IEnumerable<ApplicationUser>> GetOperatorsAsync(int idOffice)
        {
            var result = await _officesOperatorsRepository.ListAsync(
                new OfficeWithOperatorsSpecification(idOffice, x => x.ApplicationUser));

            var operators = new List<ApplicationUser>();

            foreach (var item in result)
            {
                operators.Add(item.ApplicationUser);
            }

            return operators;
        }

        public async Task<int> GetOperatorsCountAsync(int id)
        {
            return await _officesOperatorsRepository.CountAsync(
                new OfficeWithOperatorsSpecification(id));
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksAsync(int idOffice)
        {
            var result = await _officesTasksRepository.ListAsync(
                new OfficeWithTaskSpecification(idOffice, x => x.Task));

            var tasks = new List<TaskEntity>();

            foreach (var item in result)
            {
                tasks.Add(item.Task);
            }

            return tasks;
        }

        public async Task<int> GetTasksCountAsync(int id)
        {
            return await _officesTasksRepository.CountAsync(
                new OfficeWithTaskSpecification(id));
        }

        public async Task<bool> HasOperator(int idOffice, string idOperator)
        {
            return await _officesOperatorsRepository.ExistAsync(
                new OfficeWithOperatorsSpecification(idOperator, idOffice));
        }

        public async Task<bool> HasTask(int idOffice, int idTask)
        {
            return await _officesTasksRepository.ExistAsync(
                new OfficeWithTaskSpecification(idTask, idOffice));
        }

        public async Task<OperationResult> RemoveOperatorAsync(string idOperator, int idOffice)
        {
            var operatorRemove = await _officesOperatorsRepository.GetByIdAsync(idOffice, idOperator);

            if(operatorRemove == null)
            {
                var operationResult = new OperationResult { Succeeded = false };
                operationResult.Errors.Add("No se encontro el registro a eliminar");
                return operationResult;
            }

            await _officesOperatorsRepository.DeleteAsync(operatorRemove);

            return new OperationResult { Succeeded = true };
        }

        public async Task<OperationResult> RemoveTaskAsync(int idTask, int idOffice)
        {
            var taskRemove = await _officesTasksRepository.GetByIdAsync(idOffice, idTask);

            if (taskRemove == null)
            {
                var operationResult = new OperationResult { Succeeded = false };
                operationResult.Errors.Add("No se encontro el registro a eliminar");
                return operationResult;
            }

            await _officesTasksRepository.DeleteAsync(taskRemove);

            return new OperationResult { Succeeded = true };
        }

    }
}
