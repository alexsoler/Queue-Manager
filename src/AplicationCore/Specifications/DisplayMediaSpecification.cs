using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class DisplayMediaSpecification : BaseSpecification<DisplayMedia>
    {
        public DisplayMediaSpecification() : base(null)
        {
            AddInclude(x => x.Media);
            ApplyOrderBy(x => x.Order);
        }
    }
}
