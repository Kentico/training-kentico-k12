using System.Web.Mvc;
using Kentico.DI;
using Kentico.Dto.Page;

namespace MedioClinic.Controllers
{
    public class DoctorsController : BaseController
    {
        public DoctorsController(IBusinessDependencies dependencies) : base(dependencies)
        {
        }

        public ActionResult Index()
        {
            var model = GetPageViewModel(new PageMetadataDto()
            {
                Title = "Doctors"
            });

            return View(model);
        }
    }
}