using System.Collections.Generic;
using Kentico.Dto.Company;

namespace Kentico.Repository.Company
{
    public interface ICompanyServiceRepository : IRepository
    {
         IEnumerable<CompanyServiceDto> GetCompanyServices();
    }
}
