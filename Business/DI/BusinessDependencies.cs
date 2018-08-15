using Business.Repository.Company;
using Business.Repository.Culture;
using Business.Repository.Menu;
using Business.Repository.Social;
using Business.Services.Context;

namespace Business.DI
{
    public class BusinessDependencies : IBusinessDependencies
    {
        public IMenuRepository MenuRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public ICultureService CultureRepository { get; }
        public ISiteContextService SiteContextService { get; }
        public ISocialLinkRepository SocialLinkRepository { get; }

        public BusinessDependencies(
            IMenuRepository menuRepository,
            ICompanyRepository companyRepository,
            ICultureService cultureRepository,
            ISiteContextService siteContextService,
            ISocialLinkRepository socialLinkRepository
            )
        {
            MenuRepository = menuRepository;
            CompanyRepository = companyRepository;
            CultureRepository = cultureRepository;
            SiteContextService = siteContextService;
            SocialLinkRepository = socialLinkRepository;
        }
    }
}
