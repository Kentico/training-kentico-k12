using System.Linq;
using CMS.DocumentEngine.Types.MedioClinic;
using Business.Dto.Home;
using Business.Services.Query;

namespace Business.Repository.Home
{
    public class HomeSectionRepository : BaseRepository, IHomeSectionRepository
    {

        public HomeSectionRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public HomeSectionDto GetHomeSection()
        {
            return DocumentQueryService.GetDocuments<HomeSection>()
                .Columns("Title", "Text", "Button", "NodeSiteId")
                .TopN(1)
                .ToList()
                .Select(m => new HomeSectionDto()
                {
                    Title = m.Title,
                    Text = m.Text,
                    LinkText = m.Button
                })
                .FirstOrDefault();
        }
    }
}
