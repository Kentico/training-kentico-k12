using MedioClinic.Dto.Doctors;

namespace MedioClinic.Repository.Doctor
{
    public interface IDoctorSectionRepository : IRepository
    {
        DoctorSectionDto GetDoctorSection();
    }
}
