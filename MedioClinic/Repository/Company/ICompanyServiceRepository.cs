using System.Collections.Generic;
using MedioClinic.Dto.Company;

namespace MedioClinic.Repository.Company
{
    public interface ICompanyServiceRepository : IRepository
    {
         IEnumerable<CompanyServiceDto> GetCompanyServices();
    }
}
