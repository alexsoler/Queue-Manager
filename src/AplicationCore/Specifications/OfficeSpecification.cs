using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public sealed class OfficeSpecification : BaseSpecification<Office>
    {
        public OfficeSpecification(bool showOff = false)
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

        public OfficeSpecification(int idOffice)
            : base(x => x.Id == idOffice)
        {
        }

        public OfficeSpecification(int idOffice, bool withTasks = false, bool withOperators = false)
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
        }
    }
}
