using MedioClinic.Dto.Contact;

namespace MedioClinic.Repository.Contact
{
    public interface IContactSectionRepository : IRepository
    {
        ContactSectionDto GetContactSection();
    }
}
