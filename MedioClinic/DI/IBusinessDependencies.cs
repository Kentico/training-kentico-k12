using MedioClinic.Repository.Company;
using MedioClinic.Repository.Culture;
using MedioClinic.Repository.Menu;
using MedioClinic.Repository.Social;
using MedioClinic.Services.Context;

namespace MedioClinic.DI
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
