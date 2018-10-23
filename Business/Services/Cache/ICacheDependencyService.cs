using System;

namespace Business.Services.Cache
{
    public interface ICacheDependencyService : IService
    {
        string GetAndSetPageDependency(Guid guid);
    }
}
