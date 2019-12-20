using Business.Repository.Company;
using Business.Repository.Menu;
using Business.Repository.Social;
using Business.Services.Cache;
using Business.Services.Context;
using Business.Services.Culture;
using Business.Services.Errors;
using Business.Services.Localization;

namespace Business.DependencyInjection
{
    public interface IBusinessDependencies
    {
        IMenuRepository MenuRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        ICultureService CultureService { get; }
        ISiteContextService SiteContextService { get; }
        ISocialLinkRepository SocialLinkRepository { get; }
        ICacheService CacheService { get; }
        IErrorHelperService ErrorHelperService { get; set; }
        ILocalizationService LocalizationService { get; set; }
    }
}
