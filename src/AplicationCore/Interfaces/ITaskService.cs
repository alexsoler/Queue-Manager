using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ITaskService
    {
        Task<TaskEntity> AddTaskAsync(TaskEntity task);
        Task<OperationResult> RemoveTaskAsync(int id);
        Task<TaskEntity> GetTaskAsync(int id);
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task<OperationResult> EditTaskAsync(TaskEntity task);
    }
}
