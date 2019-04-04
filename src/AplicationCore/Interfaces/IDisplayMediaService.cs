using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IDisplayMediaService
    {
        Task<DisplayMedia> AddAsync(DisplayMedia displayMedia);
        Task<IEnumerable<DisplayMedia>> ListAllAsync();
        Task<OperationResult> RemoveAsync(int id);

        Task<OperationResult> UpdateOrder(int id, double order);
    }
}
