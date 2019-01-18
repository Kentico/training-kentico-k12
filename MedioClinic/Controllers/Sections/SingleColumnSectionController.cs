using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Sections;

[assembly: RegisterSection("MedioClinic.SingleColumnSection", typeof(SingleColumnSectionController), "{$MedioClinic.Section.SingleColumn.Name$}", Description = "{$MedioClinic.Section.SingleColumn.Description$}", IconClass = "icon-square")]

namespace MedioClinic.Controllers.Sections
{
    public class SingleColumnSectionController : Controller
    {
        // GET: SingleColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_SingleColumnSection");
        }
    }
}