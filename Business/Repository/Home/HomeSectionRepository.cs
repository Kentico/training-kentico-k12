using System.Linq;
using CMS.DocumentEngine.Types.Training;
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
                .AddColumns("Title", "Text", "Button")
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
