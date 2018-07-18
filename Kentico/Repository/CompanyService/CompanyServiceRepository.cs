using System.Collections.Generic;
using System.Linq;
using Kentico.Dto.CompanyService;
using Kentico.Services.Query;
using Kentico.Content.Web.Mvc;

namespace Kentico.Repository.CompanyService
{
    public class CompanyServiceRepository : BaseRepository, ICompanyServiceRepository
    {
        public CompanyServiceRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<CompanyServiceDto> GetCompanyServices()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.Training.CompanyService>()
                .Columns("CompanySectionHeader", "CompanySectionText", "CompanySectionIcon", "DocumentID")
                .Select(m => new CompanyServiceDto()
                {
                    Header = m.CompanySectionHeader,
                    Text = m.CompanySectionText,
                    IconPath = m.Fields.CompanySectionIcon.GetPath("")
                });
        }
    }
}
