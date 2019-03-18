using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    class TaskWithOffices : BaseSpecification<OfficeTask>
    {
        public TaskWithOffices(int idTask, bool includeOffice = false)
            : base(x => x.TaskId == idTask)
        {
            if (includeOffice)
            {
                AddInclude(x => x.Office);
            }
        }
    }
}
