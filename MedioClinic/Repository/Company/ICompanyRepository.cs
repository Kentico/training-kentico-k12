

using MedioClinic.Dto.Company;

namespace MedioClinic.Repository.Company
{
    public interface ICompanyRepository : IRepository
    {
        CompanyDto GetCompany();
    }
}
