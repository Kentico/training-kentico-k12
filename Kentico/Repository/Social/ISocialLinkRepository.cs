using System.Collections.Generic;
using Kentico.Dto.Social;

namespace Kentico.Repository.Social
{
    public interface ISocialLinkRepository : IRepository
    {
        IEnumerable<SocialLinkDto> GetSocialLinks();
    }
}
