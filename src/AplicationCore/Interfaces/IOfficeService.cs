using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IOfficeService
    {
        Task<int> GetOfficesCountAsync(int id);
        Task<int> GetTasksCountAsync(int id);
        Task<int> GetOperatorsCountAsync(int id);
        Task<bool> HasTask(int idOffice, int idTask);
        Task<bool> HasOperator(int idOffice, string idOperator);
        Task<Office> GetOfficeAsync(int id);
        Task<IEnumerable<Office>> GetOfficesAsync();
        Task<Office> AddOfficeAsync(Office office);
        Task<OperationResult> AddTaskAsync(int idTask, int idOffice);
        Task<OperationResult> AddTasksAsync(int[] idTasks, int idOffice);
        Task<OperationResult> AddOperatorAsync(string IdOperator, int idOffice);
        Task<OperationResult> AddOperatorsAsync(string[] IdOperators, int idOffice);
        Task<IEnumerable<TaskEntity>> GetTasksAsync(int idOffice);
        Task<IEnumerable<ApplicationUser>> GetOperatorsAsync(int idOffice);
        Task<OperationResult> RemoveTaskAsync(int idTask, int idOffice);
        Task<OperationResult> RemoveOperatorAsync(string idOperator, int idOffice);
        Task<OperationResult> DeleteOfficeAsync(int id);
        Task<OperationResult> EditOfficeAsync(Office office);
    }
}
