using Kentico.Repository.Clinic;
using Kentico.Repository.Culture;
using Kentico.Repository.Menu;
using Kentico.Services.Context;

namespace Kentico.DI
{
    public class BusinessDependencies : IBusinessDependencies
    {
        public IMenuRepository MenuRepository { get; }
        public IClinicRepository ClinicRepository { get; }
        public ICultureRepository CultureRepository { get; }
        public ISiteContextService SiteContextService { get; }

        public BusinessDependencies(
            IMenuRepository menuRepository,
            IClinicRepository clinicRepository,
            ICultureRepository cultureRepository,
            ISiteContextService siteContextService)
        {
            MenuRepository = menuRepository;
            ClinicRepository = clinicRepository;
            CultureRepository = cultureRepository;
            SiteContextService = siteContextService;
        }
    }
}
