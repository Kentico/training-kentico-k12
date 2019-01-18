using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Dto.LandingPage;
using Business.Services.Query;

namespace Business.Repository.LandingPage
{
    public class LandingPageRepository : BaseRepository, ILandingPageRepository
    {
        public LandingPageRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public LandingPageDto GetLandingPage(string pageAlias)
        {
            return DocumentQueryService.GetDocument<CMS.DocumentEngine.Types.MedioClinic.LandingPage>(pageAlias)
                .AddColumns("DocumentID", "DocumentName")
                .ToList()
                .Select(landingPage => new LandingPageDto()
                {
                    DocumentId = landingPage.DocumentID,
                    Title = landingPage.DocumentName
                })
                .FirstOrDefault();
        }
    }
}
