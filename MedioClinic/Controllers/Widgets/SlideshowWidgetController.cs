using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;

[assembly: RegisterWidget("MedioClinic.Widget.Slideshow", typeof(SlideshowWidgetController), "{$Widget.Slideshow.Name$}", Description = "{$Widget.Slideshow.Description$}", IconClass = "icon-carousel")]

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

            return PartialView("Widgets/_SlideshowWidget", new SlideshowWidgetViewModel
            {
                Images = images,
                Width = properties.Width,
                Height = properties.Height,
                EnforceDimensions = properties.EnforceDimensions,
                TransitionDelay = properties.TransitionDelay,
                TransitionSpeed = properties.TransitionSpeed,
                DisplayArrowSigns = properties.DisplayArrowSigns
            });
        }

        /// <summary>
        /// Gets document attachment images by their GUIDs.
        /// </summary>
        /// <param name="guids">GUIDs to search by.</param>
        /// <returns>The <see cref="DocumentAttachment"/> images.</returns>
        protected IEnumerable<DocumentAttachment> GetImages(IEnumerable<Guid> guids)
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