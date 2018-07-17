
using System.Collections.Generic;
using UI.Models.Menu;

namespace Kentico.Services
{
    public interface IMenuService
    {
        IEnumerable<MenuItemDto> GetMenuItems();
    }
}
