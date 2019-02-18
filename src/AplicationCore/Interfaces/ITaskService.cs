﻿using ApplicationCore.Entities;
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
        Task<IEnumerable<TaskEntity>> GetAllAsync();
    }
}
