using System.Collections.Generic;
using System.Web.Mvc;
using Kentico.DI;
using Kentico.Dto.Company;
using Kentico.Dto.Menu;
using Kentico.Dto.Page;
using Kentico.Dto.Social;
using MedioClinic.Models;

namespace MedioClinic.Controllers
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