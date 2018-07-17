
using System.Collections.Generic;
using UI.Models.Menu;

namespace Kentico.Services.Menu
{
    public interface IMenuService
    {
        IEnumerable<MenuItemDto> GetMenuItems();
    }
}
