using System.Web.Mvc;

using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("MedioClinic.General.TextWidget", typeof(TextWidgetController), "{$MedioClinic.Widget.Text.Name$}", Description = "{$MedioClinic.Widget.Text.Description$}", IconClass = "icon-l-text")]

namespace MedioClinic.Controllers.Widgets
{
    public class TextWidgetController : WidgetController<TextWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="TextWidgetController"/> class.
        /// </summary>
        public TextWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="TextWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public TextWidgetController(IWidgetPropertiesRetriever<TextWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: TextWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            return PartialView("Widgets/_TextWidget", new TextWidgetViewModel { Text = properties.Text });
        }
    }
}