using Business.Dto.Doctors;

namespace MvcTrainingMedioClinic.Models.Doctors
{
    public class DoctorDetailViewModel : IViewModel
    {
        public DoctorDto Doctor { get; set; }
    }
}