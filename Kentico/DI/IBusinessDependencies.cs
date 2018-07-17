using Kentico.Repository.Clinic;
using Kentico.Repository.Menu;

namespace Kentico.DI
{
    public interface IBusinessDependencies
    {
        IMenuRepository MenuRepository { get; }
        IClinicRepository ClinicRepository { get; }
    }
}
