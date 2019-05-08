using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class CommentSpecification : BaseSpecification<Comment>
    {
        public CommentSpecification(bool isView)
            : base(x => x.IsView == isView)
        {

        }

        public CommentSpecification(int month, int year)
            : base(x => x.CreationDate.Month.Equals(month) && x.CreationDate.Year.Equals(year))
        {

        }
    }
}
