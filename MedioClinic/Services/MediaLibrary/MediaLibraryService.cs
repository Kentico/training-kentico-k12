using System.Collections.Generic;
using System.Linq;
using CMS.MediaLibrary;
using MedioClinic.Dto.MediaLibrary;

namespace MedioClinic.Services.MediaLibrary
{
    public class MediaLibraryService : IMediaLibraryService
    {
        public IEnumerable<MediaLibraryFileDto> GetMediaLibraryFiles(string folder, string sitename, params string[] extensions)
        {
            // Gets an instance of the 'SampleMediaLibrary' media library for the current site
            var mediaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(folder, sitename);

            // Gets a collection of media files with the .jpg extension from the media library
            return MediaFileInfoProvider.GetMediaFiles()
                .WhereEquals("FileLibraryID", mediaLibrary.LibraryID)
                .WhereIn("FileExtension", extensions)
                .ToList()
                .Select(m => new MediaLibraryFileDto()
                {
                    Title = m.FileTitle,
                    Extension = m.FileExtension,
                    DirectUrl = MediaLibraryHelper.GetDirectUrl(m),
                    PermanentUrl = MediaLibraryHelper.GetPermanentUrl(m)
                });
        }
    }
}
