using System.Web.Mvc;
using Kentico.Services;
using UI.Models.Shared;

namespace MedioClinic.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IMenuService menuService) : base (menuService)
        {
        }

        public ActionResult Index()
        {
            var model = GetPageDto(new PageMetadata()
            {
                Title = "Home"
            });

            return View(model);
        }
    }
}