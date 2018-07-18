
using System.Web.Mvc;
using Kentico.DI;
using Kentico.Dto.Page;
using MedioClinic.Models.Contact;

namespace MedioClinic.Controllers
{
    public class ContactController : BaseController
    {

        public ContactController(IBusinessDependencies dependencies) : base(dependencies)
        {
        }

        public ActionResult Index()
        {
            var clinic = Dependencies.ClinicRepository.GetClinic();

            if (clinic == null)
            {
                return HttpNotFound();
            }

            var model = GetPageViewModel(new ContactViewModel()
            {
                Clinic = clinic
            }, "Contact");

            return View(model);
        }
    }
}