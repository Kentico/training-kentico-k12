using System.Collections.Generic;
using Business.Dto.Doctors;

namespace Business.Repository.Doctor
{
    public interface IDoctorRepository : IRepository
    {
        IEnumerable<DoctorDto> GetDoctors();
        DoctorDto GetDoctor(int nodeId);
    }
}
