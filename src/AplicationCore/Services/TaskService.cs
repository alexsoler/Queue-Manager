using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class TaskService : ITaskService
    {
        private readonly IAsyncRepository<TaskEntity> _taskRepository;
        private readonly IAppLogger<TaskService> _logger;

        public TaskService(IAsyncRepository<TaskEntity> taskRepository,
            IAppLogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<TaskEntity> AddTaskAsync(TaskEntity task)
        {
            _logger.LogInformation($"Se va a agregar la tarea {task.Name}");
            return await _taskRepository.AddAsync(task);
        }

        public async Task<OperationResult> EditTaskAsync(TaskEntity task)
        {
            _logger.LogInformation($"Se va a editar la tarea {task.Name}");
            var operationResult = new OperationResult();

            try
            {
                await _taskRepository.UpdateAsync(task);
                operationResult.Succeeded = true;
                _logger.LogInformation($"Se edito la tarea {task.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                operationResult.Succeeded = false;
            }

            return operationResult;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await _taskRepository.ListAllAsync();
        }

        public async Task<TaskEntity> GetTaskAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<OperationResult> RemoveTaskAsync(int id)
        {
            _logger.LogInformation($"Se va a eliminar la tarea de id {id}");
            var taskRemove = await _taskRepository.GetByIdAsync(id);

            if(taskRemove == null)
            {
                var operationResult = new OperationResult { Succeeded = false };
                operationResult.Errors.Add($"La tarea con id {id} ya se encontraba eliminada");
                _logger.LogInformation($"La tarea con id {id} ya se encontraba eliminada");
                return operationResult;
            }

            await _taskRepository.DeleteAsync(taskRemove);
            _logger.LogInformation($"Se elimino la tarea de id {id}");
            return new OperationResult { Succeeded = true };
        }
    }
}
