using CMS.MediaLibrary;
using CMS.SiteProvider;
using Kentico.Forms.Web.Mvc;
using MedioClinic.Models.FormComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: RegisterFormComponent(
    MediaLibrarySelectionComponent.Identifier, 
    typeof(MediaLibrarySelectionComponent), 
    "{$FormComponent.MediaLibrarySelection.Name$}", 
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