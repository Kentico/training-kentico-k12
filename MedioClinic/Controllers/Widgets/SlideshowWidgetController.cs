using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

[assembly: RegisterWidget("MedioClinic.Widget.Slideshow", typeof(SlideshowWidgetController), "{$MedioClinic.Widget.Slideshow.Name$}", Description = "{$MedioClinic.Widget.Slideshow.Description$}", IconClass = "icon-carousel")]

namespace MedioClinic.Controllers.Widgets
{
    public class SlideshowWidgetController : WidgetController<SlideshowWidgetProperties>
    {
        protected const int DefaultWidth = 400;
        protected const int DefaultHeight = 300;

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

            var guids = properties?
                .ImageIds?
                .Select(imageId =>
                {
                    Guid guid;

                    return Guid.TryParse(imageId, out guid) ? guid : Guid.Empty;
                })
                .Where(guid => guid != Guid.Empty);

            var images = GetImages(guids);
            //int width, height;

            if (!int.TryParse(properties.Width, out int width) || !int.TryParse(properties.Height, out int height))
            {
                width = DefaultWidth;
                height = DefaultHeight;
            }

            return PartialView("Widgets/_SlideshowWidget", new SlideshowWidgetViewModel
            {
                Images = images,
                //Width = properties.Width == 0 ? DefaultWidth : properties.Width,
                //Height = properties.Height == 0 ? DefaultHeight : properties.Height
                Width = width,
                Height = height
            });
        }

        private IEnumerable<DocumentAttachment> GetImages(IEnumerable<Guid> guids)
        {
            var page = GetPage();

            if (guids != null && guids.Any())
            {
                var guidList = guids.ToList();

                return page?
                    .AllAttachments
                    .Where(attachment => guids.Any(guid => guid == attachment.AttachmentGUID))
                    .OrderBy(attachment => guidList.IndexOf(attachment.AttachmentGUID))
                    .ToList();
            }

            return new List<DocumentAttachment>();
        }
    }
}