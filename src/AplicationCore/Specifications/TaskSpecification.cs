using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class TaskSpecification :BaseSpecification<TaskEntity>
    {
        public TaskSpecification()
            : base(null)
        {

        }
    }
}
