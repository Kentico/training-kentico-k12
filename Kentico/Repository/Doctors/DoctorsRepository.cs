using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine.Types.Training;
using Kentico.Content.Web.Mvc;
using Kentico.Dto.Doctors;
using Kentico.Services.Query;

namespace Kentico.Repository.Doctors
{
    public class DoctorsRepository : BaseRepository, IDoctorsRepository
    {

        private readonly string[] _doctorColumns = 
        {
            "NodeID", "NodeAlias", "DoctorBio", "DoctorDegree", "DoctorEmergencyShift", "DoctorFirstName",
            "DoctorLastName", "DoctorImage", "DoctorSpecialty", "DocumentID"
        };

        private Func<Doctor, DoctorDto> DoctorDtoSelect => doctor => new DoctorDto()
        {
            NodeAlias = doctor.NodeAlias,
            NodeId = doctor.NodeID,
            Bio = doctor.DoctorBio,
            Degree = doctor.DoctorDegree,
            EmergencyShift = doctor.DoctorEmergencyShift,
            FirstName = doctor.DoctorFirstName,
            LastName = doctor.DoctorLastName,
            ImagePath = doctor.Fields.Image.GetPath(),
            Specialty = doctor.DoctorSpecialty
        };

        public DoctorsRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<DoctorDto> GetDoctors()
        {
            return DocumentQueryService.GetDocuments<Doctor>()
                .Columns(_doctorColumns)
                .ToList()
                .Select(DoctorDtoSelect);
        }

        public DoctorDto GetDoctor(int nodeId)
        {
            return DocumentQueryService.GetDocument<Doctor>(nodeId)
                .Columns(_doctorColumns)
                .Select(DoctorDtoSelect)
                .FirstOrDefault();
        }
    }
}
