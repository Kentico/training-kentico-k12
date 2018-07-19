using System.Collections.Generic;
using Kentico.Dto.Doctors;

namespace MedioClinic.Models.Doctors
{
    public class DoctorsViewModel : IViewModel
    {
        public DoctorSectionDto DoctorSection { get; set; }
        public IEnumerable<DoctorDto> Doctors { get; set; }
    }
}