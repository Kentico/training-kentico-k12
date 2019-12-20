using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.MediaLibrary;
using CMS.SiteProvider;
using Kentico.Forms.Web.Mvc;

using MedioClinic.Models.FormComponents;

[assembly: RegisterFormComponent(
    MediaLibrarySelectionComponent.Identifier, 
    typeof(MediaLibrarySelectionComponent), 
    "{$FormComponent.MediaLibrarySelection.Name$}", 
    ViewName = "FormComponents/_MediaLibrarySelection",
    Description = "{$FormComponent.MediaLibrarySelection.Description$}", 
    IconClass = "icon-menu")]

namespace MedioClinic.Models.FormComponents
{
    public class MediaLibrarySelectionComponent : SelectorFormComponent<MediaLibrarySelectionProperties>
    {
        public const string Identifier = "MedioClinic.FormComponent.MediaLibrarySelection";

        protected override IEnumerable<SelectListItem> GetItems() => 
            MediaLibraryInfoProvider
                .GetMediaLibraries()
                .WhereEquals("LibrarySiteID", SiteContext.CurrentSiteID)
                .TypedResult
                .Items
                .Select(mediaLibraryInfo => new SelectListItem
                {
                    Text = mediaLibraryInfo.LibraryDisplayName,
                    Value = mediaLibraryInfo.LibraryID.ToString()
                });
    }
}