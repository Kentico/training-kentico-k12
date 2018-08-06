using Kentico.Repository.Company;
using Kentico.Repository.Culture;
using Kentico.Repository.Menu;
using Kentico.Repository.Social;
using Kentico.Services.Context;

namespace Kentico.DI
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
