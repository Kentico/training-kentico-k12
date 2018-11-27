using System.Web.Mvc;
using Business.DependencyInjection;

namespace MedioClinic.Controllers
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