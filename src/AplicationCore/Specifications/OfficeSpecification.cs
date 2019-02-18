using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public sealed class OfficeSpecification : BaseSpecification<Office>
    {
        public OfficeSpecification()
            : base(null)
        {

        }
        public OfficeSpecification(int idOffice)
            : base(x => x.Id == idOffice)
        {
        }
    }
}
