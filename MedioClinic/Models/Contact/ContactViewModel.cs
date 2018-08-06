using System.Collections.Generic;
using Kentico.Dto.Contact;
using Kentico.Dto.Map;
using Kentico.Dto.MediaLibrary;

namespace MedioClinic.Models.Contact
{
    public class ContactViewModel : IViewModel
    {
        public ContactSectionDto ContactSection { get; set; }
        public IEnumerable<MapLocationDto> OfficeLocations { get; set; }
        public IEnumerable<MediaLibraryFileDto> MedicalCenterImages { get; set; }
    }
}