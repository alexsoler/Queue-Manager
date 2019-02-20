using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class OfficesFilterPaginatedSpecification : BaseSpecification<Office>
    {
        public OfficesFilterPaginatedSpecification(int skip, int take)
            : base(null)
        {
            ApplyPaging(skip, take);
        }
    }
}
