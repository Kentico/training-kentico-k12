using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Sections;

[assembly: RegisterSection("MedioClinic.SectionFourtySixty.TwoColumn", typeof(TwoColumnFourtySixtySectionController), "{$Section.TwoColumnFourtySixty.Name$}", Description = "{$Section.TwoColumnFourtySixty.Description$}", IconClass = "icon-l-cols-30-70")]


namespace MedioClinic.Controllers.Sections
{
    public class TwoColumnFourtySixtySectionController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("Sections/_TwoColumnFourtySixtySection");
        }
    }
}