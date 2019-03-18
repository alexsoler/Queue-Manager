using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IOperatorService
    {
        Task<IEnumerable<Office>> GetOffices(string idOperator);
    }
}
