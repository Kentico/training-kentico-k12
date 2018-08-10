using System.Collections.Generic;
using MedioClinic.Dto.Doctors;

namespace Web.Models.Doctors
{
    public class DoctorsViewModel : IViewModel
    {
        public DoctorSectionDto DoctorSection { get; set; }
        public IEnumerable<DoctorDto> Doctors { get; set; }
    }
}