using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using Business.DependencyInjection;
using Business.Dto.MediaLibrary;
using Business.Repository.MediaLibrary;
using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;
using MedioClinic.Config;

[assembly: RegisterWidget(
    "MedioClinic.Widget.Slideshow", 
    typeof(SlideshowWidgetController), 
    "{$Widget.Slideshow.Name$}", 
    Description = "{$Widget.Slideshow.Description$}", 
    IconClass = "icon-carousel")]

namespace MedioClinic.Controllers.Widgets
{
    public class SlideshowWidgetController : WidgetController<SlideshowWidgetProperties>
    {
        public IBusinessDependencies Dependencies { get; }

        public IMediaLibraryRepository MediaLibraryRepository { get; }

        public SlideshowWidgetController(IBusinessDependencies dependencies, IMediaLibraryRepository mediaLibraryRepository)
        {
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            MediaLibraryRepository = mediaLibraryRepository ?? throw new ArgumentNullException(nameof(mediaLibraryRepository));
            MediaLibraryRepository.MediaLibrarySiteName = Dependencies.SiteContextService.SiteName;
        }

        public ActionResult Index()
        {
            var properties = GetProperties();
            MediaLibraryRepository.MediaLibraryName = properties.MediaLibraryName;
            var images = MediaLibraryRepository.GetMediaLibraryDtos(properties.ImageGuids);

            return PartialView("Widgets/_SlideshowWidget", new SlideshowWidgetViewModel
            {
                MediaLibraryViewModel = new Models.MediaLibraryViewModel
                {
                    LibraryName = properties.MediaLibraryName,
                    AllowedImageExtensions = AppConfig.AllowedImageExtensions
                },
                ImageDtos = MirrorOriginalSequence(properties.ImageGuids, images),
                Width = properties.Width,
                Height = properties.Height,
                EnforceDimensions = properties.EnforceDimensions,
                TransitionDelay = properties.TransitionDelay,
                TransitionSpeed = properties.TransitionSpeed,
                DisplayArrowSigns = properties.DisplayArrowSigns
            });
        }

        protected IEnumerable<MediaLibraryFileDto> MirrorOriginalSequence(
            IEnumerable<Guid> originalSequence, IEnumerable<MediaLibraryFileDto> distinctSequence)
        {
            if (originalSequence == null)
            {
                yield break;
            }

            foreach (var guid in originalSequence)
            {
                var match = distinctSequence.FirstOrDefault(dto => dto.Guid.Equals(guid));

                if (match != null)
                {
                    yield return match;
                }
            }
        }
    }
}