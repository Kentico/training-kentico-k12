using Business.Dto.Doctors;

namespace Business.Repository.Doctor
{
    public interface IDoctorSectionRepository : IRepository
    {
        DoctorSectionDto GetDoctorSection();
    }
}
