using ApplicationCore.Entities;
using ApplicationCore.Enums;
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

        public MediaSpecification(TypeFiles type)
            : base(x => type == TypeFiles.Image ? x.Img : 
            (type == TypeFiles.Video ? x.Video : x.Audio))
        {

        }

        public MediaSpecification(int month, int year)
            : base(x => x.CreationDate.Month.Equals(month) && x.CreationDate.Year.Equals(year))
        {

        }
    }
}
