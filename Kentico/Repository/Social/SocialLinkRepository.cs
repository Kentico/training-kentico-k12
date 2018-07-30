using System.Collections.Generic;
using System.Linq;
using Kentico.Content.Web.Mvc;
using Kentico.Dto.Social;
using Kentico.Services.Query;

namespace Kentico.Repository.Social
{
    public class SocialLinkRepository : BaseRepository, ISocialLinkRepository
    {
        public SocialLinkRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<SocialLinkDto> GetSocialLinks()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.Training.SocialLink>()
                .AddColumns("Title", "Url", "Icon", "DocumentID")
                .OrderByAscending("NodeOrder")
                .ToList()
                .Select(m => new SocialLinkDto()
                {
                    Url = m.Url,
                    Title = m.Title,
                    IconPath = m.Fields.Icon.GetPath("")
                });
        }
    }
}
