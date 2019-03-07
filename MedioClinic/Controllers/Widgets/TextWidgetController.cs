using System.Web.Mvc;

using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget(
    "MedioClinic.Widget.Text", 
    typeof(TextWidgetController), 
    "{$Widget.Text.Name$}", 
    Description = "{$Widget.Text.Description$}", 
    IconClass = "icon-l-text")]

namespace MedioClinic.Controllers.Widgets
{
    public class TextWidgetController : WidgetController<TextWidgetProperties>
    {
        public ActionResult Index()
        {
            var properties = GetProperties();

            return PartialView("Widgets/_TextWidget", new TextWidgetViewModel { Text = properties.Text });
        }
    }
}