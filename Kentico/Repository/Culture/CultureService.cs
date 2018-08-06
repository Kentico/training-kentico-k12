using System.Collections.Generic;
using System.Linq;
using CMS.SiteProvider;
using Kentico.Dto.Culture;
using Kentico.Services.Context;

namespace Kentico.Repository.Culture
{
    public class CultureRepository : ICultureService
    {
        private ISiteContextService SiteContextService { get; }

        public CultureRepository(ISiteContextService siteContextService)
        {
            SiteContextService = siteContextService;
        }

        public IEnumerable<CultureDto> GetSiteCultures()
        {
            return CultureSiteInfoProvider.GetSiteCultures(SiteContextService.SiteName).Items.Select(m =>
                new CultureDto()
                {
                    CultureCode = m.CultureCode,
                    CultureName = m.CultureName,
                    CultureShortName = m.CultureShortName
                }
            );
        }
    }
}
