using Business.Repository.Company;
using Business.Repository.Culture;
using Business.Repository.Menu;
using Business.Repository.Social;
using Business.Services.Cache;
using Business.Services.Context;

namespace Business.DI
{
    public class BusinessDependencies : IBusinessDependencies
    {
        public IMenuRepository MenuRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public ICultureRepository CultureRepository { get; }
        public ISiteContextService SiteContextService { get; }
        public ISocialLinkRepository SocialLinkRepository { get; }
        public ICacheDependencyService CacheDependencyService { get; }

        public BusinessDependencies(
            IMenuRepository menuRepository,
            ICompanyRepository companyRepository,
            ICultureRepository cultureRepository,
            ISiteContextService siteContextService,
            ISocialLinkRepository socialLinkRepository,
            ICacheDependencyService cacheDependencyService
            )
        {
            MenuRepository = menuRepository;
            CompanyRepository = companyRepository;
            CultureRepository = cultureRepository;
            SiteContextService = siteContextService;
            SocialLinkRepository = socialLinkRepository;
            CacheDependencyService = cacheDependencyService;
        }
    }
}
