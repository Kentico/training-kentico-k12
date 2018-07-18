using Kentico.Repository.Clinic;
using Kentico.Repository.Culture;
using Kentico.Repository.Menu;
using Kentico.Services.Context;

namespace Kentico.DI
{
    public interface IBusinessDependencies
    {
        IMenuRepository MenuRepository { get; }
        IClinicRepository ClinicRepository { get; }
        ICultureRepository CultureRepository { get; }
        ISiteContextService SiteContextService { get; }
    }
}
