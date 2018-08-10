using System.Collections.Generic;
using MedioClinic.Dto.Contact;
using MedioClinic.Dto.Map;
using MedioClinic.Dto.MediaLibrary;

namespace Web.Models.Contact
{
    public class ContactViewModel : IViewModel
    {
        public ContactSectionDto ContactSection { get; set; }
        public IEnumerable<MapLocationDto> OfficeLocations { get; set; }
        public IEnumerable<MediaLibraryFileDto> MedicalCenterImages { get; set; }
    }
}