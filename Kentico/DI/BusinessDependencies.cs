using Kentico.Repository.Company;
using Kentico.Repository.Menu;
using Kentico.Repository.Social;
using Kentico.Services.Context;
using Kentico.Services.Culture;

namespace Kentico.DI
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
