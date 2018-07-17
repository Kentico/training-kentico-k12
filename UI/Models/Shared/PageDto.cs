using System.Collections.Generic;
using UI.Models.Menu;

namespace UI.Models.Shared
{
    public class PageDto 
    {
        public IEnumerable<MenuItemDto> MenuItems { get; set; }
        public PageMetadata Metadata { get; set; }
    }

    public class PageDto<TDto> : PageDto where TDto : IDto
    {
        public TDto Data { get; set; }
    }
}
