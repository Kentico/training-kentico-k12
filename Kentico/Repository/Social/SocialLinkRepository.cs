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
                .AddColumns("SocialLinkTitle", "SocialLinkUrl", "SocialLinkIcon", "DocumentID")
                .OrderByAscending("NodeOrder")
                .ToList()
                .Select(m => new SocialLinkDto()
                {
                    Url = m.SocialLinkUrl,
                    Title = m.SocialLinkTitle,
                    IconPath = m.Fields.Icon.GetPath("")
                });
        }
    }
}
