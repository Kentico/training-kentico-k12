using Kentico.Repository.Clinic;
using Kentico.Repository.Menu;
using Kentico.Services.Context;
using Kentico.Services.Culture;

namespace Kentico.DI
{
    public class BusinessDependencies : IBusinessDependencies
    {
        public IMenuRepository MenuRepository { get; }
        public IClinicRepository ClinicRepository { get; }
        public ICultureService CultureRepository { get; }
        public ISiteContextService SiteContextService { get; }

        public BusinessDependencies(
            IMenuRepository menuRepository,
            IClinicRepository clinicRepository,
            ICultureService cultureRepository,
            ISiteContextService siteContextService)
        {
            MenuRepository = menuRepository;
            ClinicRepository = clinicRepository;
            CultureRepository = cultureRepository;
            SiteContextService = siteContextService;
        }
    }
}
