using System.Collections.Generic;
using Business.Dto.Company;

namespace Business.Repository.Company
{
    public interface ICompanyServiceRepository : IRepository
    {
         IEnumerable<CompanyServiceDto> GetCompanyServices();
    }
}
