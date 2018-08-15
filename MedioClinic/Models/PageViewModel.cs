using System.Collections.Generic;
using Business.Dto.Company;
using Business.Dto.Culture;
using Business.Dto.Menu;
using Business.Dto.Page;
using Business.Dto.Social;

namespace MedioClinic.Models
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
