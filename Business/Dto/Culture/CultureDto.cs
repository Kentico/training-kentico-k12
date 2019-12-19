using System;

namespace Business.Dto.Culture
{
    /// <summary>
    /// A lightweight representation of Kentico's <see cref="CMS.SiteProvider.CultureSiteInfo"/> objects.
    /// </summary>
    public class CultureDto : IDto
    {
        public Guid CultureGuid { get; set; }

        public string CultureCode { get; set; }

        public string CultureName { get; set; }

        public string CultureShortName { get; set; }
    }
}
