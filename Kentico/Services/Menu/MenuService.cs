using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Training;
using Kentico.Services.Query;
using UI.Models.Menu;

namespace Kentico.Services.Menu
{
    public class MenuService : IMenuService
    {
        private IDocumentQueryService DocumentQueryService { get; }

        public MenuService(IDocumentQueryService documentQueryService)
        {
            DocumentQueryService = documentQueryService;
        }

        public IEnumerable<MenuItemDto> GetMenuItems()
        {
            return DocumentQueryService.GetDocuments<MenuContainerItem>()
                .Path("/Menu-items", PathTypeEnum.Children)
                .Columns("Caption", "Controller", "Action")
                .OrderByAscending("NodeOrder")
                .Select(m => new MenuItemDto()
                {
                    Action = m.Action,
                    Caption = m.Caption,
                    Controller = m.Controller
                });

            /*
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

*/
        }
    }
}
