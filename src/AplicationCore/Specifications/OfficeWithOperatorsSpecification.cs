using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Specifications
{
    public sealed class OfficeWithOperatorsSpecification : BaseSpecification<OfficeOperator>
    {
        public OfficeWithOperatorsSpecification(string operatorId, int officeId, bool ignoreQueryFilter = false)
            : base(x => x.OfficeId == officeId && x.ApplicationUserId == operatorId)
        {
            if(ignoreQueryFilter)
                AddIgnoreQueryFilter();
        }

        public OfficeWithOperatorsSpecification(int officeId)
            : base(x => x.OfficeId == officeId)
        {
        }

        public OfficeWithOperatorsSpecification(int officeId, Expression<Func<OfficeOperator, object>> expressionInclude)
            : base(x => x.OfficeId == officeId)
        {
            AddInclude(expressionInclude);
        }
    }
}
