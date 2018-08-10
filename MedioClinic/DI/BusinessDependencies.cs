using MedioClinic.Repository.Company;
using MedioClinic.Repository.Culture;
using MedioClinic.Repository.Menu;
using MedioClinic.Repository.Social;
using MedioClinic.Services.Context;

namespace MedioClinic.DI
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
