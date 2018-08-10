using System.Collections.Generic;
using System.Web.Mvc;
using MedioClinic.DI;
using MedioClinic.Dto.Company;
using MedioClinic.Dto.Menu;
using MedioClinic.Dto.Page;
using MedioClinic.Dto.Social;
using Web.Models;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected IBusinessDependencies Dependencies { get; }
        protected BaseController(IBusinessDependencies dependencies)
        {
            Dependencies = dependencies;
        }

        public PageViewModel GetPageViewModel(string title) 
        {
            return new PageViewModel()
            {
                MenuItems = Dependencies.MenuRepository.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = GetPageMetadata(title),
                Company = GetCompany(),
                Cultures = Dependencies.CultureRepository.GetSiteCultures(),
                SocialLinks = GetSocialLinks(),
            };
        }

        public PageViewModel<TViewModel> GetPageViewModel<TViewModel>(TViewModel data, string title) where TViewModel : IViewModel
        {
            return new PageViewModel<TViewModel>()
            {
                MenuItems = Dependencies.MenuRepository.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = GetPageMetadata(title),
                Company = GetCompany(),
                Cultures = Dependencies.CultureRepository.GetSiteCultures(),
                SocialLinks = GetSocialLinks(),
                Data = data
            };
        }

        private IEnumerable<SocialLinkDto> GetSocialLinks()
        {
            return Dependencies.SocialLinkRepository.GetSocialLinks();
        }

        private PageMetadataDto GetPageMetadata(string title)
        {
            return new PageMetadataDto()
            {
                Title = title,
                CompanyName = Dependencies.SiteContextService.SiteName
            };
        }

        private CompanyDto GetCompany()
        {
            return Dependencies.CompanyRepository.GetCompany();
        }
    }
}