using System;
using System.Web;
using Business.Services.Context;
using CMS.Helpers;

namespace Business.Services.Cache
{
    public class CacheDependencyService : ICacheDependencyService
    {
        public ISiteContextService SiteContextService { get; }

        public CacheDependencyService(ISiteContextService siteContextService)
        {
            SiteContextService = siteContextService;
        }

        public string GetAndSetPageDependency(Guid guid)
        {
            var dependencyCacheKey = $"nodeguid|{SiteContextService.SiteName.ToLowerInvariant()}|{guid}";

            CacheHelper.EnsureDummyKey(dependencyCacheKey);
            HttpContext.Current.Response.AddCacheItemDependency(dependencyCacheKey);

            return dependencyCacheKey;
        }
    }
}
