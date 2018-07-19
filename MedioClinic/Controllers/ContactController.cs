
using System.Web.Mvc;
using Kentico.DI;
using Kentico.Repository.Contact;
using Kentico.Repository.Map;
using MedioClinic.Models.Contact;

namespace MedioClinic.Controllers
{
    public class ContactController : BaseController
    {

        private IContactSectionRepository ContactSectionRepository { get; }
        private IMapRepository MapRepository { get; }

        public ContactController(
            IBusinessDependencies dependencies, 
            IContactSectionRepository contactSectionRepository,
            IMapRepository mapRepository
            ) : base(dependencies)
        {
            ContactSectionRepository = contactSectionRepository;
            MapRepository = mapRepository;
        }

        public ActionResult Index()
        {
            var contactSection = ContactSectionRepository.GetContactSection();

            if (contactSection == null)
            {
                return HttpNotFound();
            }

            var model = GetPageViewModel(new ContactViewModel()
            {
                ContactSection = contactSection,
                OfficeLocations = MapRepository.GetOfficeLocations()
            }, contactSection.Header);

            return View(model);
        }
    }
}