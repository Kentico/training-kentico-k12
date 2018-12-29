using System;
using System.Web;
using Business.Services.Context;
using CMS.Helpers;

namespace Business.Services.Cache
{
    public class CacheService : ICacheService
    {
        public ISiteContextService SiteContextService { get; }

        // Injects a service which holds the site name and current culture code name
        public CacheService(ISiteContextService siteContextService)
        {
            SiteContextService = siteContextService;
        }

        // Returns a dummy cache key for pages ("nodes") in the content tree
        // "nodes|<site name>|<generated class name>|< type of cached data >"
        // which is touched when pages are modified
        public string GetNodesCacheDependencyKey(string className, CacheDependencyType dependencyType)
        {
            return $"nodes|{SiteContextService.SiteName}|{className}|{dependencyType}".ToLowerInvariant();
        }

        // Returns a dummy cache key for a page ("node") in the content tree identified by its "NodeGuid"
        // "nodeguid|<site name>| <given node in the content tree>"
        // which is touched when the page is modified
        public string GetNodeCacheDependencyKey(Guid nodeGuid)
        {
            return $"nodeguid|{SiteContextService.SiteName}|{nodeGuid}".ToLowerInvariant();
        }

        public void SetOutputCacheDependency(Guid nodeGuid)
        {
            var dependencyCacheKey = GetNodeCacheDependencyKey(nodeGuid);

            // Ensures that the dummy key cache item exists
            CacheHelper.EnsureDummyKey(dependencyCacheKey);

            // Sets cache dependency to clear the cache when there is any change to node with given GUID in Kentico
            HttpContext.Current.Response.AddCacheItemDependency(dependencyCacheKey);
        }

        public TData Cache<TData>(Func<TData> dataLoadMethod, int cacheForMinutes, string cacheItemName, string cacheDependencyKey) 
        {
            var cacheSettings = new CacheSettings(cacheForMinutes, cacheItemName, SiteContextService.SiteName, SiteContextService.CurrentSiteCulture)
            {
                GetCacheDependency = () => CacheHelper.GetCacheDependency(cacheDependencyKey.ToLowerInvariant())
            };

            return CacheHelper.Cache(dataLoadMethod, cacheSettings);
        }
    }
}
