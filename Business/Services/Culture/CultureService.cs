using System.Collections.Generic;
using System.Linq;

using CMS.SiteProvider;

using Business.Dto.Culture;
using Business.Services.Context;

namespace Business.Services.Culture
{
    public class CultureService : BaseService, ICultureService
    {
        private ISiteContextService SiteContextService { get; }

        public CultureService(ISiteContextService siteContextService)
        {
            SiteContextService = siteContextService;
        }

        public IEnumerable<CultureDto> GetSiteCultures() =>
            CultureSiteInfoProvider.GetSiteCultures(SiteContextService.SiteName)
                .Items
                .Select(culture =>
                    new CultureDto()
                    {
                        CultureGuid = culture.CultureGUID,
                        CultureCode = culture.CultureCode,
                        CultureName = culture.CultureName,
                        CultureShortName = culture.CultureShortName
                    }
            );
    }
}
