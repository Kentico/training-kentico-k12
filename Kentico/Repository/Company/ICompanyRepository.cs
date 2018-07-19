

using Kentico.Dto.Company;

namespace Kentico.Repository.Company
{
    public interface ICompanyRepository : IRepository
    {
        CompanyDto GetCompany();
    }
}
