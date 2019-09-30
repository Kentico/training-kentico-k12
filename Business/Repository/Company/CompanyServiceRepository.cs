using System.Collections.Generic;
using System.Linq;
using Business.Dto.Company;
using Business.Services.Query;

namespace Business.Repository.Company
{
    public class CompanyServiceRepository : BaseRepository, ICompanyServiceRepository
    {
        public CompanyServiceRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<CompanyServiceDto> GetCompanyServices()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.MedioClinic.CompanyService>()
                .AddColumns("Header", "Text", "Icon")
                .OrderByAscending("NodeOrder")
                .Select(m => new CompanyServiceDto()
                {
                    Header = m.Header,
                    Text = m.Text,
                    Icon = m.Fields.Icon
                });
        }
    }
}
