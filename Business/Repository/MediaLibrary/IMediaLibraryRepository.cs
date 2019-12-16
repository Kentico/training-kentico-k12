using System;
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

        Guid AddMediaLibraryFile(string filePath, string libraryFolderPath = null);

        MediaLibraryFileDto GetMediaLibraryDto(Guid fileGuid);

        IEnumerable<MediaLibraryFileDto> GetMediaLibraryDtos(params Guid[] fileGuids);

        IEnumerable<MediaLibraryFileDto> GetMediaLibraryDtos(params string[] extensions);
    }
}
