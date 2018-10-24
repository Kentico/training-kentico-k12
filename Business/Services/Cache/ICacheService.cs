using System;

namespace Business.Services.Cache
{
    public interface ICacheService : IService
    {
        /// <summary>
        /// Gets nodes dependency key for given type and action
        /// </summary>
        /// <param name="className">Class name of page type</param>
        /// <param name="dependencyType">Dependency type</param>
        /// <returns>Cache dependency key</returns>
        string GetNodesCacheDependencyKey(string className, CacheDependencyType dependencyType);

        /// <summary>
        /// Gets cache dependency key for given node
        /// </summary>
        /// <param name="nodeGuid">NodeGuid of page</param>
        /// <returns>Dependency key</returns>
        string GetNodeCacheDependencyKey(Guid nodeGuid);

        /// <summary>
        /// Sets cache dependencies for given node in current http context and ensures Output cache up to date
        /// </summary>
        /// <param name="nodeGuid">Guid of the node</param>
        /// <returns>Dependency key</returns>
        void SetOutputCacheDependency(Guid nodeGuid);

        /// <summary>
        /// Caches the result of given function using Kentico Cache & dependencies
        /// </summary>
        /// <typeparam name="TData">Type of cached Data</typeparam>
        /// <param name="dataLoadMethod">Function to get data</param>
        /// <param name="cacheName">Name of cache item</param>
        /// <param name="cacheForMinutes">Number of minutes data should stay in cache</param>
        /// <param name="cacheDependencyKey">Dependency key</param>
        /// <returns>Data</returns>
        TData Cache<TData>(Func<TData> dataLoadMethod, int cacheForMinutes, string cacheName, string cacheDependencyKey);
    }
}
