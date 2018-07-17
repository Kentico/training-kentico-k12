using System.Collections.Generic;
using System.Web.Mvc;
using Kentico.Services;
using UI;
using UI.Models.Menu;
using UI.Models.Shared;

namespace MedioClinic.Controllers
{
    public class BaseController : Controller
    {
        protected IMenuService MenuService { get; }
        protected BaseController(IMenuService menuService)
        {
            MenuService = menuService;
        }

        public PageDto GetPageDto(PageMetadata metadata) 
        {
            return new PageDto()
            {
                MenuItems = MenuService.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = metadata ?? new PageMetadata(),
            };
        }

        public PageDto<TDto> GetPageDto<TDto>(TDto data, PageMetadata metadata) where TDto : IDto
        {
            return new PageDto<TDto>()
            {
                MenuItems = MenuService.GetMenuItems() ?? new List<MenuItemDto>(),
                Metadata = metadata ?? new PageMetadata(),
                Data = data
            };
        }

    }
}