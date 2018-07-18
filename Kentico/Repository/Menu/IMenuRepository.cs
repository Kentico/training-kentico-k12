
using System.Collections.Generic;
using Kentico.Dto.Menu;

namespace Kentico.Repository.Menu
{
    public interface IMenuRepository : IRepository
    {
        IEnumerable<MenuItemDto> GetMenuItems();
    }
}
