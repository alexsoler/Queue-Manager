using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Specifications
{
    public sealed class OfficeSpecification : BaseSpecification<Office>
    {
        public OfficeSpecification(bool showOff)
            : base(x => x.Activo == false)
        {
            if (showOff)
                AddIgnoreQueryFilter();
        }

        public OfficeSpecification(string filterName = "")
            : base(x => string.IsNullOrEmpty(filterName) ||
                x.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase))
        {

        }

        public OfficeSpecification(int idOffice, bool withTasks = false,
            bool withOperators = false, bool ignoreFilter = false)
            :base(x => x.Id == idOffice)
        {
            if(withTasks)
            {
                AddInclude(x => x.OfficeTasks);
            }
            if(withOperators)
            {
                AddInclude(x => x.OfficeOperators);
            }
            if(ignoreFilter)
            {
                AddIgnoreQueryFilter();
            }
        }

        public OfficeSpecification(int idTask)
            : base(x => x.OfficeTasks.Any(t => t.TaskId == idTask))
        {
            AddInclude(x => x.OfficeTasks);
            AddInclude(X => X.OfficeOperators);
        }
    }
}
