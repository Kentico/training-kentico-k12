using System.Collections.Generic;
using System.Web.Mvc;
using Kentico.DI;
using UI.Dto.Page;
using UI.Models.Menu;
using UI.ViewModel;

namespace MedioClinic.Controllers
{
    public class BaseController : Controller
    {
        protected IBusinessDependencies Dependencies { get; }
        protected BaseController(IBusinessDependencies dependencies)
        {
            Dependencies = dependencies;
        }

        public PageViewModel GetPageViewModel(PageMetadataDto metadata) 
        {
            return new PageViewModel()
            {
                MenuItems = Dependencies.MenuRepository.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = metadata ?? new PageMetadataDto(),
                Footer = GetFooter()
            };
        }

        public PageViewModel<TViewModel> GetPageViewModel<TViewModel>(TViewModel data, PageMetadataDto metadata) where TViewModel : IViewModel
        {
            return new PageViewModel<TViewModel>()
            {
                MenuItems = Dependencies.MenuRepository.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = metadata ?? new PageMetadataDto(),
                Footer = GetFooter(),
                Data = data
            };
        }

        private PageFooterDto GetFooter()
        {
            var clinic = Dependencies.ClinicRepository.GetClinic();

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