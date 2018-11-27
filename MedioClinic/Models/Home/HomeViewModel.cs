using System.Collections.Generic;
using Business.Dto.Company;
using Business.Dto.Home;

namespace MedioClinic.Models.Home
{
    public class HomeViewModel : IViewModel
    {
        public IEnumerable<CompanyServiceDto> CompanyServices { get; set; }

        public HomeSectionDto HomeSection { get; set; }
    }
}