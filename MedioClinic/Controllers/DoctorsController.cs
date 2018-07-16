using System.Web.Mvc;

namespace MedioClinic.Controllers
{
    public class DoctorsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}