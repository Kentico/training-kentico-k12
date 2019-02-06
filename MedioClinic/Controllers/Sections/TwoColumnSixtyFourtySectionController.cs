using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Sections;

[assembly: RegisterSection("MedioClinic.Section.TwoColumnSixtyFourty", typeof(TwoColumnSixtyFourtySectionController), "{$Section.TwoColumnSixtyFourty.Name$}", Description = "{$Section.TwoColumnSixtyFourty.Description$}", IconClass = "icon-l-cols-70-30")]


namespace MedioClinic.Controllers.Sections
{
    public class TwoColumnSixtyFourtySectionController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("Sections/_TwoColumnSixtyFourtySection");
        }
    }
}