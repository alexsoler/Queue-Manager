using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Specifications
{
    public sealed class OfficeWithTaskSpecification : BaseSpecification<OfficeTask>
    {
        public OfficeWithTaskSpecification(int idTask, int idOffice)
            : base(x => x.TaskId == idTask && x.OfficeId == idOffice)
        {
        }

        public OfficeWithTaskSpecification(int idOffice)
            : base(x => x.OfficeId == idOffice)
        {
            
        }

        public OfficeWithTaskSpecification(int idOffice, Expression<Func<OfficeTask, object>> expressionInclude)
            : base(x => x.OfficeId == idOffice)
        {
            AddInclude(expressionInclude);
        }
    }
}
