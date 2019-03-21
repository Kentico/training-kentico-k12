using System.Collections.Generic;
using Business.Dto.MediaLibrary;

namespace Business.Services.MediaLibrary
{
    public interface IMediaLibraryService
    {
        /// <summary>
        /// Wrapper around Kentico API for retrieving media stored in media libraries. 
        /// Fetches files from the given folder.
        /// </summary>
        /// <param name="folder">Codename of the folder</param>
        /// <param name="sitename">Sitename</param>
        /// <param name="extensions">Allowed extensions (e.g. '.png', '.jpg')</param>
        /// <returns></returns>
        IEnumerable<MediaLibraryFileDto> GetMediaLibraryFiles(string folder, string sitename, params string[] extensions);
    }
}
