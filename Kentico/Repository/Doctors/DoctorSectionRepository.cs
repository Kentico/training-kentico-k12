using System;
using System.Linq;
using CMS.DocumentEngine.Types.Training;
using Kentico.Dto.Doctors;
using Kentico.Dto.Sections;
using Kentico.Services.Query;

namespace Kentico.Repository.Doctors
{
    public class DoctorSectionRepository : BaseRepository, IDoctorSectionRepository
    {

        public DoctorSectionRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public DoctorSectionDto GetDoctorSection()
        {
            return DocumentQueryService.GetDocuments<DoctorSection>()
                .Columns("DoctorSectionName")
                .TopN(1)
                .ToList()
                .Select(m => new DoctorSectionDto()
                {
                    Header = m.DoctorSectionName
                })
                .FirstOrDefault();
        }
    }
}
