using System.Collections.Generic;
using Kentico.Dto.Doctors;

namespace Kentico.Repository.Doctor
{
    public interface IDoctorRepository : IRepository
    {
        IEnumerable<DoctorDto> GetDoctors();
        DoctorDto GetDoctor(int nodeId);
    }
}
