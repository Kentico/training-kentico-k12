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
            "NodeID", "NodeAlias", "Bio", "Degree", "EmergencyShift", "FirstName",
            "LastName", "Image", "Specialty", "DocumentID"
        };

        private Func<Doctor, DoctorDto> DoctorDtoSelect => doctor => new DoctorDto()
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

        public DoctorsRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<DoctorDto> GetDoctors()
        {
            return DocumentQueryService.GetDocuments<Doctor>()
                .AddColumns(_doctorColumns)
                .ToList()
                .Select(DoctorDtoSelect);
        }

        public DoctorDto GetDoctor(int nodeId)
        {
            return DocumentQueryService.GetDocument<Doctor>(nodeId)
                .AddColumns(_doctorColumns)
                .Select(DoctorDtoSelect)
                .FirstOrDefault();
        }
    }
}
