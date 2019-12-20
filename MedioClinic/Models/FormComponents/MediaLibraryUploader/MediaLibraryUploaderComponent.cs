using System;

using CMS.Core;
using CMS.Membership;
using CMS.SiteProvider;
using Kentico.Forms.Web.Mvc;

using Business.Repository.MediaLibrary;
using MedioClinic.Models.FormComponents;

[assembly: RegisterFormComponent(
    "MedioClinic.FormComponent.MediaLibraryUploader",
    typeof(MediaLibraryUploaderComponent),
    "{$FormComponent.MediaLibraryUploader.Name$}",
    ViewName = "FormComponents/_MediaLibraryUploader",
    Description = "{$FormComponent.MediaLibraryUploader.Description$}",
    IconClass = "icon-picture")]

namespace MedioClinic.Models.FormComponents
{
    public class MediaLibraryUploaderComponent : FormComponent<MediaLibraryUploaderProperties, string>
    {
        public MediaLibraryUploaderComponent()
        {
            MediaLibraryRepository = new MediaLibraryRepository();
        }

        [BindableProperty]
        public string FileGuidAsString
        {
            get => FileGuid?.ToString() ?? string.Empty;

            set
            {
                var parsed = Guid.TryParse(value, out Guid guid);

                if (parsed)
                {
                    FileGuid = guid;
                }
            }
        }

        [BindableProperty]
        public string FileName { get; set; }

        public Guid? FileGuid { get; set; }

        public bool ShowViewFileLink =>
            MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(ModuleName.BIZFORM, "ReadData");

        protected IMediaLibraryRepository MediaLibraryRepository { get; set; }

        public string FilePermanentUrl
        {
            get
            {
                var parsed = int.TryParse(Properties?.MediaLibraryId, out int libraryId);

                if (parsed && FileGuid != null)
                {
                    MediaLibraryRepository.MediaLibraryId = libraryId;
                    MediaLibraryRepository.MediaLibrarySiteName = SiteContext.CurrentSiteName;
                    var dto = MediaLibraryRepository.GetMediaLibraryDto(FileGuid.Value);

                    return dto?.PermanentUrl;
                }

                return null;
            }
        }

        public override string GetValue() => FileGuidAsString;

        public override void SetValue(string value)
        {
            FileGuidAsString = value;
        }
    }
}