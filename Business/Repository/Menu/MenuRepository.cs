using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.MedioClinic;
using Business.Dto.Menu;
using Business.Services.Query;

namespace Business.Repository.Menu
{
    public class MenuRepository : BaseRepository, IMenuRepository
    {
        public MenuRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<MenuItemDto> GetMenuItems()
        {
            return DocumentQueryService.GetDocuments<MenuContainerItem>()
                .Path("/Menu-items", PathTypeEnum.Children)
                .Columns("Caption", "Controller", "Action", "NodeSiteId")
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
