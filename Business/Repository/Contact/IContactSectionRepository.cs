using Business.Dto.Contact;

namespace Business.Repository.Contact
{
    public interface IContactSectionRepository : IRepository
    {
        ContactSectionDto GetContactSection();
    }
}
