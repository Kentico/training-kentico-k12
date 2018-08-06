using System.Collections.Generic;
using Kentico.Dto.MediaLibrary;

namespace Kentico.Services.MediaLibrary
{
    public interface IMediaLibraryService : IService
    {
        IEnumerable<MediaLibraryFileDto> GetMediaLibraryFiles(string folder, string sitename, params string[] extensions);
    }
}
