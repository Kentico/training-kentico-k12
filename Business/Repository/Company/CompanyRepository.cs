using System.Linq;
using Business.Dto.Company;
using Business.Services.Query;

namespace Business.Repository.Company
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public CompanyRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public CompanyDto GetCompany()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.MedioClinic.Company>()
                .AddColumns("CompanyName", "Street", "City", "Country",
            "ZipCode", "PhoneNumber", "Email", "DocumentID")
                .TopN(1)
                .ToList()
                .Select(m =>
                {
                    var splitCountry = m.Country.Split(';');

                    string country;
                    string state = null;
                    if (splitCountry.Length == 2)
                    {
                        country = splitCountry[0];
                        state = splitCountry[1];
                    }
                    else
                    {
                        country = m.Country;
                    }

                    return new CompanyDto()
                    {
                        Name = m.CompanyName,
                        City = m.City,
                        Street = m.Street,
                        Country = country,
                        State = state,
                        Email = m.Email,
                        PhoneNumber = m.PhoneNumber,
                        ZipCode = m.ZipCode
                    };
                })
                .FirstOrDefault();
        }
    }
}
