using System.Collections.Generic;
using Business.Dto.Contact;
using Business.Dto.Map;
using Business.Dto.MediaLibrary;

namespace MedioClinic.Models.Contact
{
    public class ContactViewModel : IViewModel
    {
        public ContactSectionDto ContactSection { get; set; }
        public IEnumerable<MapLocationDto> OfficeLocations { get; set; }
        public IEnumerable<MediaLibraryFileDto> MedicalCenterImages { get; set; }
    }
}