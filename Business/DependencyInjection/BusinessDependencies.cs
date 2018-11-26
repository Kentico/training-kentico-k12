using Business.Repository.Company;
using Business.Repository.Culture;
using Business.Repository.Menu;
using Business.Repository.Social;
using Business.Services.Cache;
using Business.Services.Context;

namespace Business.DependencyInjection
{
    public class BusinessDependencies : IBusinessDependencies
    {
        public IMenuRepository MenuRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public ICultureRepository CultureRepository { get; }
        public ISiteContextService SiteContextService { get; }
        public ISocialLinkRepository SocialLinkRepository { get; }
        public ICacheService CacheService { get; }

        public BusinessDependencies(
            IMenuRepository menuRepository,
            ICompanyRepository companyRepository,
            ICultureRepository cultureRepository,
            ISiteContextService siteContextService,
            ISocialLinkRepository socialLinkRepository,
            ICacheService cacheDependencyService
            )
        {
            MenuRepository = menuRepository;
            CompanyRepository = companyRepository;
            CultureRepository = cultureRepository;
            SiteContextService = siteContextService;
            SocialLinkRepository = socialLinkRepository;
            CacheService = cacheDependencyService;
        }
    }
}
