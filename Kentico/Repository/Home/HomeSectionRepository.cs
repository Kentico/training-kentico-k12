using System.Linq;
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
                .AddColumns("Header", "Text", "Button")
                .TopN(1)
                .ToList()
                .Select(m => new HomeSectionDto()
                {
                    Title = m.Header,
                    Text = m.Text,
                    LinkText = m.Button
                })
                .FirstOrDefault();
        }
    }
}
