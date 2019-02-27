using System.Collections.Generic;
using Business.Dto.Company;

namespace Business.Repository.Company
{
    public interface ICompanyServiceRepository
    {
         IEnumerable<CompanyServiceDto> GetCompanyServices();
    }
}
