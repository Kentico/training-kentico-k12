using System.Linq;
using System.Web.Mvc;
using Business.Services.Context;
using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;

[assembly: RegisterWidget(
    "MedioClinic.Widget.Image", 
    typeof(ImageWidgetController), 
    "{$Widget.Image.Name$}", 
    Description = "{$Widget.Image.Description$}", 
    IconClass = "icon-picture")]

namespace MedioClinic.Controllers.Widgets
{
    public class ImageWidgetController : WidgetController<ImageWidgetProperties>
    {
        public ActionResult Index()
        {
            var properties = GetProperties();
            var image = GetImage(properties);

            return PartialView("Widgets/_ImageWidget", new ImageWidgetViewModel
            {
                Image = image
            });
        }

        /// <summary>
        /// Gets a page attachment image by the properties.
        /// </summary>
        /// <param name="properties">Properties with the GUID.</param>
        /// <returns>The <see cref="DocumentAttachment"/> with the image.</returns>
        protected DocumentAttachment GetImage(ImageWidgetProperties properties)
        {
            var page = GetPage();
            
            return page?.AllAttachments.FirstOrDefault(x => x.AttachmentGUID == properties.ImageGuid);
        }
    } 
}