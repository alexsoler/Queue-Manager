using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class MediasFilterPaginatedSpecification : BaseSpecification<Media>
    {
        public MediasFilterPaginatedSpecification(int skip, int take)
            : base(null)
        {
            ApplyPaging(skip, take);
        }
    }
}
