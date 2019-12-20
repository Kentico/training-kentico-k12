using System;
using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using Business.DependencyInjection;
using Business.Repository.MediaLibrary;
using MedioClinic.Controllers.Widgets;
using MedioClinic.Models.Widgets;
using MedioClinic.Config;

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
        public IMediaLibraryRepository MediaLibraryRepository { get; }

        public IBusinessDependencies Dependencies { get; }

        public ImageWidgetController(IMediaLibraryRepository mediaLibraryRepository, IBusinessDependencies dependencies)
        {
            MediaLibraryRepository = mediaLibraryRepository ?? throw new ArgumentNullException(nameof(mediaLibraryRepository));
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        public ActionResult Index()
        {
            var properties = GetProperties();
            bool hasImage = false;
            string imageUrl = null;
            MediaLibraryRepository.MediaLibraryName = properties.MediaLibraryName;
            MediaLibraryRepository.MediaLibrarySiteName = Dependencies.SiteContextService.SiteName;

            if (properties.ImageGuid != Guid.Empty && !string.IsNullOrEmpty(properties.MediaLibraryName))
            {
                hasImage = true;
                imageUrl = MediaLibraryRepository.GetMediaLibraryDto(properties.ImageGuid)?.DirectUrl;
            }

            return PartialView("Widgets/_ImageWidget", new ImageWidgetViewModel
            {
                HasImage = hasImage,
                ImageUrl = imageUrl,

                MediaLibraryViewModel = new Models.MediaLibraryViewModel
                {
                    LibraryName = MediaLibraryRepository.MediaLibraryName,
                    LibrarySiteName = MediaLibraryRepository.MediaLibrarySiteName,
                    AllowedImageExtensions = AppConfig.AllowedImageExtensions
                }
            });
        }
    }
}