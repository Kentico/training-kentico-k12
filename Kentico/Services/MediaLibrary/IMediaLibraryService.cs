using System.Collections.Generic;
using Kentico.Dto.MediaLibrary;

namespace Kentico.Services.MediaLibrary
{
    public interface IMediaLibraryService : IService
    {
        /// <summary>
        /// Wrapper around Kentico's Media library. Used to fetch files from given folder.
        /// </summary>
        /// <param name="folder">Codename of the folder</param>
        /// <param name="sitename">Sitename</param>
        /// <param name="extensions">Allowed extensions (e.g. '.png', '.jpg')</param>
        /// <returns></returns>
        IEnumerable<MediaLibraryFileDto> GetMediaLibraryFiles(string folder, string sitename, params string[] extensions);
    }
}
