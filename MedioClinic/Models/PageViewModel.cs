using System.Collections.Generic;

using Business.DependencyInjection;
using Business.Dto.Company;
using Business.Dto.Culture;
using Business.Dto.Menu;
using Business.Dto.Page;
using Business.Dto.Social;

namespace MedioClinic.Models
{
    /// <summary>
    /// Base class 
    /// </summary>
    public class PageViewModel : IViewModel
    {
        public IEnumerable<MenuItemDto> MenuItems { get; set; }
        public PageMetadataDto Metadata { get; set; }
        public CompanyDto Company { get; set; }
        public IEnumerable<CultureDto> Cultures { get; set; }
        public IEnumerable<SocialLinkDto> SocialLinks { get; set; }

        public static PageViewModel GetPageViewModel(string title, IBusinessDependencies dependencies) =>
            new PageViewModel()
            {
                MenuItems = dependencies.MenuRepository.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = GetPageMetadata(title, dependencies),
                Company = GetCompany(dependencies),
                Cultures = dependencies.CultureService.GetSiteCultures(),
                SocialLinks = GetSocialLinks(dependencies),
            };

        protected static PageMetadataDto GetPageMetadata(string title, IBusinessDependencies dependencies) =>
            new PageMetadataDto()
            {
                Title = title,
                CompanyName = dependencies.SiteContextService.SiteName
            };

        protected static CompanyDto GetCompany(IBusinessDependencies dependencies) =>
            dependencies.CompanyRepository.GetCompany();

        protected static IEnumerable<SocialLinkDto> GetSocialLinks(IBusinessDependencies dependencies) =>
            dependencies.SocialLinkRepository.GetSocialLinks();
    }

    public class PageViewModel<TViewModel> : PageViewModel where TViewModel : IViewModel
    {
        public TViewModel Data { get; set; }

        public static PageViewModel<TViewModel> GetPageViewModel(TViewModel data, string title, IBusinessDependencies dependencies) =>
            new PageViewModel<TViewModel>()
            {
                MenuItems = dependencies.MenuRepository.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = GetPageMetadata(title, dependencies),
                Company = GetCompany(dependencies),
                Cultures = dependencies.CultureService.GetSiteCultures(),
                SocialLinks = GetSocialLinks(dependencies),
                Data = data
            };
    }
}
