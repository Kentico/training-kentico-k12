using Kentico.Dto.Contact;

namespace Kentico.Repository.Contact
{
    public interface IContactSectionRepository : IRepository
    {
        ContactSectionDto GetContactSection();
    }
}
