using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;

[assembly: RegisterWidget("MedioClinic.Widget.Slideshow", typeof(SlideshowWidgetController), "{$MedioClinic.Widget.Slideshow.Name$}", Description = "{$MedioClinic.Widget.Slideshow.Description$}", IconClass = "icon-carousel")]

namespace MedioClinic.Controllers.Widgets
{
    public class SlideshowWidgetController : WidgetController<SlideshowWidgetProperties>
    {
        public SlideshowWidgetController()
        {
        }

        public SlideshowWidgetController(IWidgetPropertiesRetriever<SlideshowWidgetProperties> propertiesRetriever, ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }

        // GET: SlideshowWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            var images = GetImages(properties);

            return PartialView("Widgets/_SlideshowWidget", new SlideshowWidgetViewModel
            {
                Images = images
            });
        }

        private IEnumerable<DocumentAttachment> GetImages(SlideshowWidgetProperties properties)
        {
            var page = GetPage();
            return page?
                .AllAttachments
                .Where(attachment => properties.ImageIds.Any(guid => guid == attachment.AttachmentGUID));
        }
    }
}