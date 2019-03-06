using CMS.SiteProvider;
using Kentico.Forms.Web.Mvc;
using MedioClinic.Models.FormComponents;

[assembly: RegisterFormComponent(
    "MedioClinic.FormComponent.MediaLibraryUploader", 
    typeof(MediaLibraryUploaderComponent), 
    "{$FormComponent.MediaLibraryUploader.Name$}", 
    Description = "{$FormComponent.MediaLibraryUploader.Description$}", 
    IconClass = "icon-picture")]

namespace MedioClinic.Models.FormComponents
{
    public class MediaLibraryUploaderComponent : FormComponent<MediaLibraryUploaderProperties, string>
    {
        [BindableProperty]
        public string FileGuid { get; set; } = string.Empty;

        public string SiteName => SiteContext.CurrentSiteName;

        public override string GetValue() => FileGuid;

        public override void SetValue(string value)
        {
            FileGuid = value;
        }
    }
}