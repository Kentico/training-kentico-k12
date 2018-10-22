using System.Collections.Generic;
using System.Linq;
using CMS.MediaLibrary;
using Business.Dto.MediaLibrary;

namespace Business.Services.MediaLibrary
{
    public class MediaLibraryService : IMediaLibraryService
    {
        public IEnumerable<MediaLibraryFileDto> GetMediaLibraryFiles(string folder, string sitename, params string[] extensions)
        {
            // Gets an instance of the media library for the current site
            var mediaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(folder, sitename);

            // Gets a collection of media files and their extensions from the media library
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
