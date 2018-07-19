using System.Linq;
using CMS.DocumentEngine.Types.Training;
using Kentico.Dto.Contact;
using Kentico.Services.Query;

namespace Kentico.Repository.Contact
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
                .Columns("Header", "Subheader", "Text")
                .ToList()
                .Select(m => new ContactSectionDto()
                {
                    Header = m.Header,
                    Subheader = m.Subheader,
                    Text = m.Text,

                })
                .FirstOrDefault();
        }
    }
}
