using System.Collections.Generic;
using Business.Dto.Social;

namespace Business.Repository.Social
{
    public interface ISocialLinkRepository : IRepository
    {
        IEnumerable<SocialLinkDto> GetSocialLinks();
    }
}
