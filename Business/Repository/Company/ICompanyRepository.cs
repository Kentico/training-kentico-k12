

using Business.Dto.Company;

namespace Business.Repository.Company
{
    public interface ICompanyRepository : IRepository
    {
        CompanyDto GetCompany();
    }
}
