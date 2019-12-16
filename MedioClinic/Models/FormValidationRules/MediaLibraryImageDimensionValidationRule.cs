using System;

using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using Kentico.Forms.Web.Mvc;

using MedioClinic.Models.FormValidationRules;

[assembly: RegisterFormValidationRule(
    "MedioClinic.ValidationRule.MediaLibraryImageDimension", 
    typeof(MediaLibraryImageDimensionValidationRule), 
    "{$ValidationRule.MediaLibraryImageDimension.Name$}", 
    Description = "{$ValidationRule.MediaLibraryImageDimension.Description$}")]

namespace MedioClinic.Models.FormValidationRules
{
    [Serializable]
    public class MediaLibraryImageDimensionValidationRule : ValidationRule<string>
    {
        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$ValidationRule.MediaLibraryImageDimension.MinimumWidth$}", Order = 0)]
        public int MinimumWidth { get; set; }

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$ValidationRule.MediaLibraryImageDimension.MaximumWidth$}", Order = 1)]
        public int MaximumWidth { get; set; }

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$ValidationRule.MediaLibraryImageDimension.MinimumHeight$}", Order = 2)]
        public int MinimumHeight { get; set; }

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$ValidationRule.MediaLibraryImageDimension.MaximumHeight$}", Order = 3)]
        public int MaximumHeight { get; set; }

        public override string GetTitle()
        {
            return $"{ResHelper.GetString("ValidationRule.MediaLibraryImageDimension.MinimumWidth")}: {MinimumWidth}. " +
                $"{ResHelper.GetString("ValidationRule.MediaLibraryImageDimension.MaximumWidth")}: {MaximumWidth}. " +
                $"{ResHelper.GetString("ValidationRule.MediaLibraryImageDimension.MinimumHeight")}: {MinimumHeight}. " +
                $"{ResHelper.GetString("ValidationRule.MediaLibraryImageDimension.MaximumHeight")}: {MaximumHeight}.";
        }

        protected override bool Validate(string value)
        {
            Guid guid;
            guid = Guid.TryParse(value, out guid) ? guid : Guid.Empty;

            if (guid != Guid.Empty)
            {
                var mediaFileInfo = MediaFileInfoProvider.GetMediaFileInfo(guid, SiteContext.CurrentSiteName);

                return mediaFileInfo != null
                    && MinimumWidth <= mediaFileInfo.FileImageWidth
                    && mediaFileInfo.FileImageWidth <= MaximumWidth
                    && MinimumHeight <= mediaFileInfo.FileImageHeight
                    && mediaFileInfo.FileImageHeight <= MaximumHeight;
            }

            return false;
        }
    }
}