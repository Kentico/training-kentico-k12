using System.Collections.Generic;
using MedioClinic.Dto.Company;
using MedioClinic.Dto.Culture;
using MedioClinic.Dto.Menu;
using MedioClinic.Dto.Page;
using MedioClinic.Dto.Social;

namespace Web.Models
{

    /// <summary>
    /// Base class 
    /// </summary>
    public class PageViewModel  : IViewModel
    {
        public IEnumerable<MenuItemDto> MenuItems { get; set; }
        public PageMetadataDto Metadata { get; set; }
        public CompanyDto Company { get; set; }
        public IEnumerable<CultureDto> Cultures { get; set; }
        public IEnumerable<SocialLinkDto> SocialLinks { get; set; }
    }

    public class PageViewModel<TViewModel> : PageViewModel where TViewModel : IViewModel
    {
        public TViewModel Data { get; set; }
    }
}
