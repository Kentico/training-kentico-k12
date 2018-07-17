
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Training;
using UI.Models;
using UI.Models.Menu;

namespace Kentico.Services
{
    public class MenuService : IMenuService
    {

        public IEnumerable<MenuItemDto> GetMenuItems()
        {
            return MenuContainerItemProvider.GetMenuContainerItems()
                .Path("/Menu-items", PathTypeEnum.Children)
                .Columns("Caption", "Controller", "Action")
                .OrderByAscending("NodeOrder")
                .Select(m => new MenuItemDto()
                {
                    Action = m.Action,
                    Caption = m.Caption,
                    Controller = m.Controller
                });
        }
    }
}
