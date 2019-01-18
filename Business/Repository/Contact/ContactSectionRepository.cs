using System.Linq;
using CMS.DocumentEngine.Types.MedioClinic;
using Business.Dto.Contact;
using Business.Services.Query;

namespace Business.Repository.Contact
{
    public class ContactSectionRepository : BaseRepository, IContactSectionRepository
    {

        public ContactSectionRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public ContactSectionDto GetContactSection()
        {
            return DocumentQueryService.GetDocuments<ContactSection>()
                .TopN(1)
                .Columns("Title", "Subtitle", "Text", "NodeSiteId")
                .ToList()
                .Select(m => new ContactSectionDto()
                {
                    Header = m.Title,
                    Subheader = m.Subtitle,
                    Text = m.Text,

                })
                .FirstOrDefault();
        }
    }
}
