using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class OfficesFilterPaginatedSpecification : BaseSpecification<Office>
    {
        public OfficesFilterPaginatedSpecification(int skip, int take, string filterName = "")
            : base(x => string.IsNullOrEmpty(filterName) || 
                x.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase))
        {
            ApplyPaging(skip, take);
        }
    }
}
