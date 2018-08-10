using System.Collections.Generic;
using MedioClinic.Dto.Company;
using MedioClinic.Dto.Sections;

namespace Web.Models.Home
{
    public class HomeViewModel : IViewModel
    {
        public IEnumerable<CompanyServiceDto> CompanyServices { get; set; }

        public HomeSectionDto HomeSection { get; set; }
    }
}