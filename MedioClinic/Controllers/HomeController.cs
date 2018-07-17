using System.Web.Mvc;
using Kentico.DI;
using UI.Dto.Page;

namespace MedioClinic.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IBusinessDependencies dependencies) : base (dependencies)
        {
        }

        public ActionResult Index()
        {
            var model = GetPageViewModel(new PageMetadataDto()
            {
                Title = "Home"
            });

            return View(model);
        }
    }
}