using System.Web.Mvc;
using Kentico.Services;
using Kentico.Services.Menu;
using UI.Models.Shared;

namespace MedioClinic.Controllers
{
    public class DoctorsController : BaseController
    {
        public DoctorsController(IMenuService menuService) : base(menuService)
        {
        }

        public ActionResult Index()
        {
            var model = GetPageDto(new PageMetadata()
            {
                Title = "Doctors"
            });

            return View(model);
        }
    }
}