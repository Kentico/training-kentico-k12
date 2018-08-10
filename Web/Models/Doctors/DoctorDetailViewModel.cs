using MedioClinic.Dto.Doctors;

namespace Web.Models.Doctors
{
    public class DoctorDetailViewModel : IViewModel
    {
        public DoctorDto Doctor { get; set; }
    }
}