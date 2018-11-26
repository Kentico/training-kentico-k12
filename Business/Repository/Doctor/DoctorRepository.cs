using System;
using System.Collections.Generic;
using System.Linq;
using Business.Dto.Doctors;
using Business.Services.Context;
using Business.Services.Query;

namespace Business.Repository.Doctor
{
    public class DoctorRepository : BaseRepository, IDoctorRepository
    {

        private readonly string[] _doctorColumns = 
        {
            // Defines database columns for retrieving data
            // NodeGuid is retrieved automatically
            "NodeID", "NodeAlias", "Bio", "Degree", "EmergencyShift", "FirstName",
            "LastName", "Photo", "Specialty", "DocumentID"
        };

        private Func<CMS.DocumentEngine.Types.Training.Doctor, DoctorDto> DoctorDtoSelect => doctor => new DoctorDto()
        {
            NodeAlias = doctor.NodeAlias,
            NodeGuid = doctor.NodeGUID,
            NodeId = doctor.NodeID,
            Bio = doctor.Bio,
            Degree = doctor.Degree,
            EmergencyShift = doctor.EmergencyShift,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Photo = doctor.Fields.Photo,
            Specialty = doctor.Specialty
        };

        ISiteContextService SiteContextService { get; }

        public DoctorRepository(IDocumentQueryService documentQueryService, ISiteContextService siteContextService) : base(documentQueryService)
        {
            SiteContextService = siteContextService;
        }

        public IEnumerable<DoctorDto> GetDoctors()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.Training.Doctor>()
                .AddColumns(_doctorColumns)
                .ToList()
                .Select(DoctorDtoSelect);
        }

        public DoctorDto GetDoctor(Guid nodeGuid)
        {
            return DocumentQueryService.GetDocument<CMS.DocumentEngine.Types.Training.Doctor>(nodeGuid)
                .AddColumns(_doctorColumns)
                .Select(DoctorDtoSelect)
                .FirstOrDefault();
        }
    }
}
