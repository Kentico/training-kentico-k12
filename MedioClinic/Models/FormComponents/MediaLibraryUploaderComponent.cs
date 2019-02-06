using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business.Services.Context;
using CMS.SiteProvider;
using Kentico.Forms.Web.Mvc;
using MedioClinic.Models.FormComponents;

[assembly: RegisterFormComponent("MedioClinic.FormComponent.MediaLibraryUploader", typeof(MediaLibraryUploaderComponent), "{$FormComponent.MediaLibraryUploader.Name$}", Description = "{$FormComponent.MediaLibraryUploader.Description$}", IconClass = "icon-picture")]

namespace MedioClinic.Models.FormComponents
{
    public class MediaLibraryUploaderComponent : FormComponent<MediaLibraryUploaderProperties, string>
    {
        [BindableProperty]
        public string FileGuid { get; set; } = string.Empty;

        [BindableProperty]
        public int PageId { get; set; }

        public string SiteName => SiteContext.CurrentSiteName;

        public override bool CustomAutopostHandling => true;

        public override string GetValue()
        {
            return FileGuid;
        }

        public override void SetValue(string value)
        {
            FileGuid = value;
        }
    }
}