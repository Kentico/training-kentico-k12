
using System.Collections.Generic;
using UI.Models.Menu;

namespace Kentico.Repository.Menu
{
    public interface IMenuRepository : IRepository
    {
        IEnumerable<MenuItemDto> GetMenuItems();
    }
}
