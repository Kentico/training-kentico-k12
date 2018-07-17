using System.Collections.Generic;
using UI.Dto.Page;
using UI.Models.Menu;

namespace UI.ViewModel
{
    public class PageViewModel 
    {
        public IEnumerable<MenuItemDto> MenuItems { get; set; }
        public PageMetadataDto Metadata { get; set; }
        public PageFooterDto Footer { get; set; }
    }

    public class PageViewModel<TViewModel> : PageViewModel where TViewModel : IViewModel
    {
        public TViewModel Data { get; set; }
    }
}
