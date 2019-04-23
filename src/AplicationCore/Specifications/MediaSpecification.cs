using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class MediaSpecification : BaseSpecification<Media>
    {
        public MediaSpecification()
            : base(x => !x.Used)
        {

        }
    }
}
