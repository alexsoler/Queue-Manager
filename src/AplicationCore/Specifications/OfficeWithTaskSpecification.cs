using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Specifications
{
    public sealed class OfficeWithTaskSpecification : BaseSpecification<OfficeTask>
    {
        public OfficeWithTaskSpecification(int idTask, int idOffice, bool ignoreQueryFilter = false)
            : base(x => x.TaskId == idTask && x.OfficeId == idOffice)
        {
            if (ignoreQueryFilter)
                AddIgnoreQueryFilter();
        }

        public OfficeWithTaskSpecification(int idOffice, bool includeTask = false)
            : base(x => x.OfficeId == idOffice)
        {
            if (includeTask)
                AddInclude(x => x.Task);
        }
    }
}
