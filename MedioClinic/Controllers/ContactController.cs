using System.Web.Mvc;
using Business.DependencyInjection;
using Business.Repository.Contact;
using Business.Repository.Map;
using Business.Services.MediaLibrary;
using MedioClinic.Models.Contact;

namespace MedioClinic.Controllers
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