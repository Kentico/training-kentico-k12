using Kentico.Dto.Doctors;

namespace Kentico.Repository.Doctor
{
    public interface IDoctorSectionRepository : IRepository
    {
        DoctorSectionDto GetDoctorSection();
    }
}
