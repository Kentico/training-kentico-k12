﻿using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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

            return PartialView("Widgets/_SlideshowWidget", new SlideshowWidgetViewModel
            {
                Images = images,
                Width = properties.Width,
                Height = properties.Height,
                TransitionDelay = properties.TransitionDelay,
                TransitionSpeed = properties.TransitionSpeed,
                DisplayArrowSigns = properties.DisplayArrowSigns
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