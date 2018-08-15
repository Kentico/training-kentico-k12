
using System.Collections.Generic;
using Business.Dto.Menu;

namespace Business.Repository.Menu
{
    public interface IMenuRepository : IRepository
    {
        IEnumerable<MenuItemDto> GetMenuItems();
    }
}
