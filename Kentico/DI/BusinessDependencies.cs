using Kentico.Repository.Clinic;
using Kentico.Repository.Menu;

namespace Kentico.DI
{
    public class BusinessDependencies : IBusinessDependencies
    {
        public IMenuRepository MenuRepository { get; }
        public IClinicRepository ClinicRepository { get; }

        public BusinessDependencies(
            IMenuRepository menuRepository,
            IClinicRepository clinicRepository)
        {
            MenuRepository = menuRepository;
            ClinicRepository = clinicRepository;
        }
    }
}
