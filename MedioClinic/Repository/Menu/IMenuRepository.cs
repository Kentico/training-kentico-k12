
using System.Collections.Generic;
using MedioClinic.Dto.Menu;

namespace MedioClinic.Repository.Menu
{
    public interface IMenuRepository : IRepository
    {
        IEnumerable<MenuItemDto> GetMenuItems();
    }
}
