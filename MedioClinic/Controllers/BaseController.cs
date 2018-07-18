using System.Collections.Generic;
using System.Web.Mvc;
using Kentico.DI;
using Kentico.Dto.Menu;
using Kentico.Dto.Page;
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
                Footer = GetFooter(),
                Cultures = Dependencies.CultureRepository.GetSiteCultures(),
            };
        }

        public PageViewModel<TViewModel> GetPageViewModel<TViewModel>(TViewModel data, string title) where TViewModel : IViewModel
        {
            return new PageViewModel<TViewModel>()
            {
                MenuItems = Dependencies.MenuRepository.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = GetPageMetadata(title),
                Footer = GetFooter(),
                Cultures = Dependencies.CultureRepository.GetSiteCultures(),
                Data = data
            };
        }


        private PageMetadataDto GetPageMetadata(string title)
        {
            return new PageMetadataDto()
            {
                Title = title,
                CompanyName = Dependencies.SiteContextService.SiteName
            };
        }

        private PageFooterDto GetFooter()
        {
            var clinic = Dependencies.ClinicRepository.GetClinic();

            if (clinic == null)
            {
                return null;
            }
            return new PageFooterDto()
            {
                Email = clinic.Email,
                PhoneNumber = clinic.PhoneNumber,
                CompanyName = clinic.Name,
                FullAddress = $"{clinic.Street}, {clinic.City}, {clinic.Country}"
            };
        }
    }
}