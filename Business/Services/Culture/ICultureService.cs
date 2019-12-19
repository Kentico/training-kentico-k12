using System.Collections.Generic;
using Business.Dto.Culture;

namespace Business.Services.Culture
{
    /// <summary>
    /// Abstraction from Kentico's <see cref="CMS.SiteProvider.CultureSiteInfoProvider"/>.
    /// </summary>
    public interface ICultureService
    {
        /// <summary>
        /// Gets culture objects.
        /// </summary>
        /// <returns>A sequence of lightweight culture objects.</returns>
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
