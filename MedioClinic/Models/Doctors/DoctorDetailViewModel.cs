using Business.Dto.Doctors;

namespace MedioClinic.Models.Doctors
{
    public class DoctorDetailViewModel : IViewModel
    {
        public DoctorDto Doctor { get; set; }
    }
}