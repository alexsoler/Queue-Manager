using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class CommentsFilterPaginatedSpecification : BaseSpecification<Comment>
    {
        public CommentsFilterPaginatedSpecification(int skip, int take, bool isView)
            : base(x => x.IsView == isView)
        {
            ApplyPaging(skip, take);
        }
    }
}
