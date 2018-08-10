using System.Collections.Generic;
using MedioClinic.Dto.Social;

namespace MedioClinic.Repository.Social
{
    public interface ISocialLinkRepository : IRepository
    {
        IEnumerable<SocialLinkDto> GetSocialLinks();
    }
}
