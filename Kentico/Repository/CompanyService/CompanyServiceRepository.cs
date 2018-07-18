using System.Collections.Generic;
using System.Linq;
using Kentico.Dto.CompanyService;
using Kentico.Services.Query;
using Kentico.Content.Web.Mvc;

namespace Kentico.Repository.CompanyService
{
    public class CompanyServiceRepository : ICompanyServiceRepository
    {
        private IDocumentQueryService DocumentQueryService { get; }

        public CompanyServiceRepository(IDocumentQueryService documentQueryService)
        {
            DocumentQueryService = documentQueryService;
        }

        public IEnumerable<CompanyServiceDto> GetCompanyServices()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.Training.CompanyService>()
                .Select(m => new CompanyServiceDto()
                {
                    Header = m.CompanySectionHeader,
                    Text = m.CompanySectionText,
                    IconPath = m.CompanySectionIcon.GetPath("")
                });
        }
    }
}
