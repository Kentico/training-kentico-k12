using Kentico.Repository.Clinic;
using Kentico.Repository.Menu;
using Kentico.Services.Context;
using Kentico.Services.Culture;

namespace Kentico.DI
{
    public interface IBusinessDependencies
    {
        IMenuRepository MenuRepository { get; }
        IClinicRepository ClinicRepository { get; }
        ICultureService CultureRepository { get; }
        ISiteContextService SiteContextService { get; }
    }
}
