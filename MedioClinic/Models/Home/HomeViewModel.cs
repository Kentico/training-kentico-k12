using System.Collections.Generic;
using Kentico.Dto.CompanyService;

namespace MedioClinic.Models.Home
{
    public class HomeViewModel : IViewModel
    {
        public IEnumerable<CompanyServiceDto> CompanyServices { get; set; }
    }
}