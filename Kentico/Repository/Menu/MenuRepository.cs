using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Training;
using Kentico.Dto.Menu;
using Kentico.Services.Query;

namespace Kentico.Repository.Menu
{
    public class MenuRepository : IMenuRepository
    {
        private IDocumentQueryService DocumentQueryService { get; }

        public MenuRepository(IDocumentQueryService documentQueryService)
        {
            DocumentQueryService = documentQueryService;
        }

        public IEnumerable<MenuItemDto> GetMenuItems()
        {
            return DocumentQueryService.GetDocuments<MenuContainerItem>()
                .Path("/Menu-items", PathTypeEnum.Children)
                .AddColumns("Caption", "Controller", "Action")
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
