using System;
using System.Linq;
using CMS.DocumentEngine.Types.MedioClinic;
using Business.Dto.Doctors;
using Business.Services.Query;

namespace Business.Repository.Doctor
{
    public class DoctorSectionRepository : BaseRepository, IDoctorSectionRepository
    {

        public DoctorSectionRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public DoctorSectionDto GetDoctorSection()
        {
            return DocumentQueryService.GetDocuments<DoctorSection>()
                .AddColumns("Title")
                .TopN(1)
                .ToList()
                .Select(m => new DoctorSectionDto()
                {
                    Header = m.Title
                })
                .FirstOrDefault();
        }
    }
}
