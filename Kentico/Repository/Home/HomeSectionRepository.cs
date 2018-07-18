using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Training;
using Kentico.Dto.Sections;
using Kentico.Services.Query;

namespace Kentico.Repository.Home
{
    public class HomeSectionRepository : BaseRepository, IHomeSectionRepository
    {

        public HomeSectionRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public HomeSectionDto GetHomeSection()
        {
            return DocumentQueryService.GetDocuments<HomeSection>()
                .TopN(1)
                .ToList()
                .Select(m => new HomeSectionDto()
                {
                    Title = m.HomeHeader,
                    Text = m.HomeText,
                    LinkText = m.HomeButton
                })
                .FirstOrDefault();
        }
    }
}
