using Kentico.Dto.Doctors;

namespace Kentico.Repository.Doctors
{
    public interface IDoctorSectionRepository : IRepository
    {
        DoctorSectionDto GetDoctorSection();
    }
}
