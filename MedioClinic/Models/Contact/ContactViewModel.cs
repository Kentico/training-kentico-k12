using System.Collections.Generic;
using Kentico.Dto.Contact;
using Kentico.Dto.Map;

namespace MedioClinic.Models.Contact
{
    public class ContactViewModel : IViewModel
    {
        public ContactSectionDto ContactSection { get; set; }
        public IEnumerable<MapLocationDto> OfficeLocations { get; set; }
    }
}