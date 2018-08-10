
using System.Web.Mvc;
using MedioClinic.DI;
using MedioClinic.Repository.Contact;
using MedioClinic.Repository.Map;
using MedioClinic.Services.MediaLibrary;
using Web.Models.Contact;

namespace Web.Controllers
{
    public class ContactController : BaseController
    {

        private IContactSectionRepository ContactSectionRepository { get; }
        private IMapRepository MapRepository { get; }
        private IMediaLibraryService MediaLibraryService { get; }

        public ContactController(
            IBusinessDependencies dependencies, 
            IContactSectionRepository contactSectionRepository,
            IMapRepository mapRepository,
            IMediaLibraryService mediaLibraryService
            ) : base(dependencies)
        {
            ContactSectionRepository = contactSectionRepository;
            MapRepository = mapRepository;
            MediaLibraryService = mediaLibraryService;
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
                OfficeLocations = MapRepository.GetOfficeLocations(),
                MedicalCenterImages = MediaLibraryService.GetMediaLibraryFiles("MedicalCenters", Dependencies.SiteContextService.SiteName, ".jpg", ".png")
            }, contactSection.Header);

            return View(model);
        }
    }
}