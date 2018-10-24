using System;
using System.Web;
using Business.Services.Context;
using CMS.Helpers;

namespace Business.Services.Cache
{
    public class CacheService : ICacheService
    {
        public ISiteContextService SiteContextService { get; }

        public CacheService(ISiteContextService siteContextService)
        {
            SiteContextService = siteContextService;
        }

        public string GetNodesCacheDependencyKey(string className, CacheDependencyType dependencyType)
        {
            return $"nodes|{SiteContextService.SiteName}|{className}|{dependencyType}".ToLowerInvariant();
        }

        public string GetNodeCacheDependencyKey(Guid nodeGuid)
        {
            return $"nodeguid|{SiteContextService.SiteName}|{nodeGuid}".ToLowerInvariant();
        }

        public void SetOutputCacheDependency(Guid nodeGuid)
        {
            var dependencyCacheKey = GetNodeCacheDependencyKey(nodeGuid);

            CacheHelper.EnsureDummyKey(dependencyCacheKey);
            HttpContext.Current.Response.AddCacheItemDependency(dependencyCacheKey);
        }

        public TData Cache<TData>(Func<TData> dataLoadMethod, int cacheForMinutes, string cacheName, string cacheDependencyKey) 
        {
            var cacheSettings = new CacheSettings(60, cacheName, SiteContextService.SiteName, SiteContextService.CurrentSiteCulture)
            {
                GetCacheDependency = () => CacheHelper.GetCacheDependency(cacheDependencyKey.ToLowerInvariant())
            };

            return CacheHelper.Cache(dataLoadMethod, cacheSettings);
        }
    }
}
