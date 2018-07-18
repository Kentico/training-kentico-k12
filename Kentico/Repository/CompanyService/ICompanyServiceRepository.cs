using System.Collections.Generic;
using Kentico.Dto.CompanyService;

namespace Kentico.Repository.CompanyService
{
    public interface ICompanyServiceRepository : IRepository
    {
         IEnumerable<CompanyServiceDto> GetCompanyServices();
    }
}
