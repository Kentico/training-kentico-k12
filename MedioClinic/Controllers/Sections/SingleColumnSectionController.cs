using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Sections;

[assembly: RegisterSection("MedioClinic.Section.SingleColumn", typeof(SingleColumnSectionController), "{$Section.SingleColumn.Name$}", Description = "{$Section.SingleColumn.Description$}", IconClass = "icon-square")]

namespace MedioClinic.Controllers.Sections
{
    public class SingleColumnSectionController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("Sections/_SingleColumnSection");
        }
    }
}