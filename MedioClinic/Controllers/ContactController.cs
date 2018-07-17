
using System.Web.Mvc;
using Kentico.Services.Menu;
using UI.Models.Shared;

namespace MedioClinic.Controllers
{
    public class ContactController : BaseController
    {
        public ContactController(IMenuService menuService) : base(menuService)
        {
        }

        public ActionResult Index()
        {
            var model = GetPageDto(new PageMetadata()
            {
                Title = "Contact"
            });

            return View(model);
        }
    }
}