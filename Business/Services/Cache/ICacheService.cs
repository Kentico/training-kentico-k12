using System;

namespace Business.Services.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Gets nodes cache dependency key for any given type and action
        /// </summary>
        /// <param name="className">Class name of the page type</param>
        /// <param name="dependencyType">Dependency type</param>
        /// <returns>Cache dependency key</returns>
        string GetNodesCacheDependencyKey(string className, CacheDependencyType dependencyType);

        /// <summary>
        /// Gets cache dependency key for the given node
        /// </summary>
        /// <param name="nodeGuid">NodeGuid of page</param>
        /// <returns>Dependency key</returns>
        string GetNodeCacheDependencyKey(Guid nodeGuid);

        /// <summary>
        /// Sets cache dependencies for the given node in the current HTTP context and ensures the otput cache is up-to-date
        /// </summary>
        /// <param name="nodeGuid">Guid of the node</param>
        /// <returns>Dependency key</returns>
        void SetOutputCacheDependency(Guid nodeGuid);

        /// <summary>
        /// Caches the result of the given function using Kentico Cache & dependencies
        /// </summary>
        /// <typeparam name="TData">Type of cached Data</typeparam>
        /// <param name="dataLoadMethod">Function to get the data</param>
        /// <param name="cacheName">Name of the cache item</param>
        /// <param name="cacheForMinutes">Number of minutes data should stay in cache</param>
        /// <param name="cacheDependencyKey">Dependency key</param>
        /// <returns>Data</returns>
        TData Cache<TData>(Func<TData> dataLoadMethod, int cacheForMinutes, string cacheName, string cacheDependencyKey);
    }
}
