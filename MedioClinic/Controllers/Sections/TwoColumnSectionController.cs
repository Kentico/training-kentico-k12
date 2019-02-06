using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Sections;

[assembly: RegisterSection("MedioClinic.Section.TwoColumn", typeof(TwoColumnSectionController), "{$Section.TwoColumn.Name$}", Description = "{$Section.TwoColumn.Description$}", IconClass = "icon-l-cols-2")]


namespace MedioClinic.Controllers.Sections
{
    public class TwoColumnSectionController : Controller
    {
        // GET: TwoColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_TwoColumnSection");
        }
    }
}