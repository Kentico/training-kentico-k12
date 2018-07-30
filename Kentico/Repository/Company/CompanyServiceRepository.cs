using System.Collections.Generic;
using System.Linq;
using Kentico.Content.Web.Mvc;
using Kentico.Dto.Company;
using Kentico.Services.Query;

namespace Kentico.Repository.Company
{
    public class CompanyServiceRepository : BaseRepository, ICompanyServiceRepository
    {
        public CompanyServiceRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<CompanyServiceDto> GetCompanyServices()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.Training.CompanyService>()
                .AddColumns("Header", "Text", "Icon", "DocumentID")
                .OrderByAscending("NodeOrder")
                .ToList()
                .Select(m => new CompanyServiceDto()
                {
                    Header = m.Header,
                    Text = m.Text,
                    IconPath = m.Fields.Icon.GetPath("")
                });
        }
    }
}
