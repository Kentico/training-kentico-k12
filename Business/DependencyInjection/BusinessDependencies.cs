using Business.Repository.Company;
using Business.Repository.Menu;
using Business.Repository.Social;
using Business.Services.Cache;
using Business.Services.Context;
using Business.Services.Culture;

namespace Business.DependencyInjection
{
    public class BusinessDependencies : IBusinessDependencies
    {
        public IMenuRepository MenuRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public ICultureService CultureService { get; }
        public ISiteContextService SiteContextService { get; }
        public ISocialLinkRepository SocialLinkRepository { get; }
        public ICacheService CacheService { get; }

        public BusinessDependencies(
            IMenuRepository menuRepository,
            ICompanyRepository companyRepository,
            ICultureService cultureService,
            ISiteContextService siteContextService,
            ISocialLinkRepository socialLinkRepository,
            ICacheService cacheDependencyService
            )
        {
            MenuRepository = menuRepository;
            CompanyRepository = companyRepository;
            CultureService = cultureService;
            SiteContextService = siteContextService;
            SocialLinkRepository = socialLinkRepository;
            CacheService = cacheDependencyService;
        }
    }
}
