using System.Web.Mvc;
using Kentico.DI;

namespace MedioClinic.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IBusinessDependencies dependencies) : base (dependencies)
        {
        }

        public ActionResult Index()
        {
            var model = GetPageViewModel("Home");

            return View(model);
        }
    }
}