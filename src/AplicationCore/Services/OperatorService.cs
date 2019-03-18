using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly IAsyncRepository<OfficeOperator> _officeOperatorRepository;
        private readonly IAppLogger<OperatorService> _logger;

        public OperatorService(IAsyncRepository<OfficeOperator> officeOperatorRepository,
            IAppLogger<OperatorService> logger)
        {
            _officeOperatorRepository = officeOperatorRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Office>> GetOffices(string idOperator)
        {
            var operatorOfficeList = await _officeOperatorRepository
                .ListAsync(new OperatorWithOfficesSpecification(idOperator, includeOffice: true));

            List<Office> offices = new List<Office>();

            foreach (var item in operatorOfficeList)
            {
                offices.Add(item.Office);
            }

            return offices;
        }
    }
}
