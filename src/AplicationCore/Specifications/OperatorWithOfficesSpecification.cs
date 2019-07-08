using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class OperatorWithOfficesSpecification : BaseSpecification<OfficeOperator>
    {
        public OperatorWithOfficesSpecification(string idOperator, bool includeOffice = false)
            : base(x => x.ApplicationUserId == idOperator)
        {
            if (includeOffice)
                AddInclude(x => x.Office);
        }
    }
}
