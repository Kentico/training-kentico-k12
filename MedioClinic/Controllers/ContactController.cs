using System;
using System.Web.Mvc;

using Business.DependencyInjection;
using Business.Repository.Contact;
using Business.Repository.Map;
using Business.Repository.MediaLibrary;
using MedioClinic.Config;
using MedioClinic.Models.Contact;

namespace MedioClinic.Controllers
{
    public class ContactController : BaseController
    {
        private IContactSectionRepository ContactSectionRepository { get; }
        private IMapRepository MapRepository { get; }
        private IMediaLibraryRepository MediaLibraryRepository { get; }

        public ContactController(
            IBusinessDependencies dependencies, 
            IContactSectionRepository contactSectionRepository,
            IMapRepository mapRepository,
            IMediaLibraryRepository mediaLibraryRepository
            ) : base(dependencies)
        {
            ContactSectionRepository = contactSectionRepository ?? throw new ArgumentNullException(nameof(contactSectionRepository));
            MapRepository = mapRepository ?? throw new ArgumentNullException(nameof(mapRepository));
            MediaLibraryRepository = mediaLibraryRepository ?? throw new ArgumentNullException(nameof(mediaLibraryRepository));
            MediaLibraryRepository.MediaLibraryName = AppConfig.MedicalCentersLibrary;
            MediaLibraryRepository.MediaLibrarySiteName = Dependencies.SiteContextService.SiteName;
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
                MedicalCenterImages = MediaLibraryRepository.GetMediaLibraryDtos(".jpg", ".png")
            }, contactSection.Header);

            return View(model);
        }
    }
}