using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Content.Web.Mvc;
using Business.Dto.Doctors;
using Business.Services.Query;

namespace Business.Repository.Doctor
{
    public class DoctorRepository : BaseRepository, IDoctorRepository
    {

        private readonly string[] _doctorColumns = 
        {
            "NodeID", "NodeAlias", "Bio", "Degree", "EmergencyShift", "FirstName",
            "LastName", "Image", "Specialty", "DocumentID"
        };

        private Func<CMS.DocumentEngine.Types.Training.Doctor, DoctorDto> DoctorDtoSelect => doctor => new DoctorDto()
        {
            NodeAlias = doctor.NodeAlias,
            NodeId = doctor.NodeID,
            Bio = doctor.Bio,
            Degree = doctor.Degree,
            EmergencyShift = doctor.EmergencyShift,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            ImagePath = doctor.Fields.Image.GetPath(),
            Specialty = doctor.Specialty
        };

        public DoctorRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<DoctorDto> GetDoctors()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.Training.Doctor>()
                .AddColumns(_doctorColumns)
                .ToList()
                .Select(DoctorDtoSelect);
        }

        public DoctorDto GetDoctor(int nodeId)
        {
            return DocumentQueryService.GetDocument<CMS.DocumentEngine.Types.Training.Doctor>(nodeId)
                .AddColumns(_doctorColumns)
                .Select(DoctorDtoSelect)
                .FirstOrDefault();
        }
    }
}
