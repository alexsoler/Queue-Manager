using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class TasksFilterPaginatedSpecification : BaseSpecification<TaskEntity>
    {
        public TasksFilterPaginatedSpecification(int skip, int take)
            : base(null)
        {
            ApplyPaging(skip, take);
        }
    }
}
