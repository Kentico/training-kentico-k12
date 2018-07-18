using System.Collections.Generic;
using Kentico.Dto.Culture;
using Kentico.Dto.Menu;
using Kentico.Dto.Page;

namespace MedioClinic.Models
{
    public class PageViewModel 
    {
        public IEnumerable<MenuItemDto> MenuItems { get; set; }
        public PageMetadataDto Metadata { get; set; }
        public PageFooterDto Footer { get; set; }
        public IEnumerable<CultureDto> Cultures { get; set; }
    }

    public class PageViewModel<TViewModel> : PageViewModel where TViewModel : IViewModel
    {
        public TViewModel Data { get; set; }
    }
}
