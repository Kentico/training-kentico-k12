using System.Web.Mvc;
using MedioClinic.DI;

namespace Web.Controllers
{
    public class NotFoundController : BaseController
    {

        public NotFoundController(IBusinessDependencies dependencies) : base(dependencies)
        {
        }

        public ActionResult Index()
        {
            Response.StatusCode = 404;

            var model = GetPageViewModel("Not found");

            return View(model);
        }
       
    }
}