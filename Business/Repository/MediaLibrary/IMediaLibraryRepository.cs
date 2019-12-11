using System.Collections.Generic;

using Business.Dto.MediaLibrary;

namespace Business.Repository.MediaLibrary
{
    public interface IMediaLibraryRepository : IRepository
    {
        string MediaLibraryName { get; set; }

        string MediaLibrarySiteName { get; set; }

        int? MediaLibraryId { get; set; }

        int? MediaLibrarySiteId { get; set; }

        IEnumerable<MediaLibraryFileDto> GetMediaLibraryDtos(params string[] extensions);
    }
}
