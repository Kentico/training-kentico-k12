using System.Collections.Generic;
using MedioClinic.Dto.Doctors;

namespace MedioClinic.Repository.Doctor
{
    public interface IDoctorRepository : IRepository
    {
        IEnumerable<DoctorDto> GetDoctors();
        DoctorDto GetDoctor(int nodeId);
    }
}
