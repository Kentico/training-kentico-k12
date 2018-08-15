using Business.Repository.Company;
using Business.Repository.Culture;
using Business.Repository.Menu;
using Business.Repository.Social;
using Business.Services.Context;

namespace Business.DI
{
    public interface IBusinessDependencies
    {
        IMenuRepository MenuRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        ICultureService CultureRepository { get; }
        ISiteContextService SiteContextService { get; }
        ISocialLinkRepository SocialLinkRepository { get; }
    }
}
