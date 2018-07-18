using System.Collections.Generic;
using Kentico.Dto.Doctors;

namespace Kentico.Repository.Doctors
{
    public interface IDoctorsRepository : IRepository
    {
        IEnumerable<DoctorDto> GetDoctors();
        DoctorDto GetDoctor(int nodeId);
    }
}
