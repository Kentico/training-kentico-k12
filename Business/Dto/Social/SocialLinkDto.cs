using CMS.DocumentEngine;

namespace Business.Dto.Social
{
    public class SocialLinkDto : IDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DocumentAttachment Icon { get; set; }
    }
}
