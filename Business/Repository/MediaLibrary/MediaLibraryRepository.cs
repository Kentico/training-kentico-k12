using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.SiteProvider;

using Business.Dto.MediaLibrary;

namespace Business.Repository.MediaLibrary
{
    public class MediaLibraryRepository : IMediaLibraryRepository
    {
        private int? _mediaLibraryId;
        private int? _mediaLibrarySiteId;
        private string _mediaLibraryName;
        private string _mediaLibrarySiteName;

        public int? MediaLibraryId
        {
            get => _mediaLibraryId == null && !string.IsNullOrEmpty(_mediaLibraryName) && !string.IsNullOrEmpty(_mediaLibrarySiteName)
                ? MediaLibraryInfoProvider
                    .GetMediaLibraryInfo(_mediaLibraryName, _mediaLibrarySiteName)?
                    .LibraryID
                : _mediaLibraryId;

            set
            {
                if (value != null)
                {
                    _mediaLibraryId = value.Value;
                }
            }
        }

        public int? MediaLibrarySiteId
        {
            get => _mediaLibrarySiteId == null && !string.IsNullOrEmpty(_mediaLibrarySiteName)
                ? SiteInfoProvider.GetSiteID(_mediaLibrarySiteName)
                : _mediaLibrarySiteId;

            set
            {
                if (value != null)
                {
                    _mediaLibrarySiteId = value.Value;
                }
            }
        }

        public string MediaLibraryName
        {
            get => string.IsNullOrEmpty(_mediaLibraryName) && _mediaLibraryId.HasValue
                ? MediaLibraryInfoProvider
                    .GetMediaLibraryInfo(_mediaLibraryId.Value)?
                    .LibraryName
                : _mediaLibraryName;

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _mediaLibraryName = value;
                }
            }
        }

        public string MediaLibrarySiteName
        {
            get
            {
                if (string.IsNullOrEmpty(_mediaLibrarySiteName) && _mediaLibrarySiteId.HasValue)
                {
                    var siteId = MediaLibraryInfoProvider
                        .GetMediaLibraryInfo(_mediaLibrarySiteId.Value)?
                        .LibrarySiteID;

                    return siteId != null
                        ? SiteInfoProvider.GetSiteInfo(siteId.Value)?.SiteName
                        : null;
                }
                else
                {
                    return _mediaLibrarySiteName;
                }
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _mediaLibrarySiteName = value;
                }
            }
        }

        public Guid AddMediaLibraryFile(string filePath, string libraryFolderPath = null, bool checkPermissions = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path was not specified.", nameof(filePath));
            }

            var mediaLibraryInfo = MediaLibraryInfoProvider.GetMediaLibraryInfo(MediaLibraryName, MediaLibrarySiteName)
                ?? MediaLibraryInfoProvider.GetMediaLibraryInfo(MediaLibraryId.Value);

            if (mediaLibraryInfo == null)
            {
                throw new Exception($"The {MediaLibraryName} library was not found on the {MediaLibrarySiteName} site.");
            }

            if (checkPermissions && !mediaLibraryInfo.CheckPermissions(PermissionsEnum.Create, MediaLibrarySiteName, MembershipContext.AuthenticatedUser))
            {
                throw new PermissionException(
                    $"The user {MembershipContext.AuthenticatedUser.FullName} lacks permissions to the {MediaLibraryName} library.");
            }

            MediaFileInfo mediaFile = !string.IsNullOrEmpty(libraryFolderPath)
                ? new MediaFileInfo(filePath, mediaLibraryInfo.LibraryID, libraryFolderPath)
                : new MediaFileInfo(filePath, mediaLibraryInfo.LibraryID);

            var fileInfo = FileInfo.New(filePath);
            mediaFile.FileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
            mediaFile.FileExtension = fileInfo.Extension;
            mediaFile.FileMimeType = MimeTypeHelper.GetMimetype(fileInfo.Extension);

            mediaFile.FileSiteID = MediaLibrarySiteId.HasValue
                ? MediaLibrarySiteId.Value
                : SiteContext.CurrentSiteID;

            mediaFile.FileLibraryID = mediaLibraryInfo.LibraryID;
            mediaFile.FileSize = fileInfo.Length;
            MediaFileInfoProvider.SetMediaFileInfo(mediaFile);

            return mediaFile.FileGUID;
        }

        public MediaLibraryFileDto GetMediaLibraryDto(Guid fileGuid)
        {
            var mediaFileInfo = MediaFileInfoProvider.GetMediaFileInfo(fileGuid, MediaLibrarySiteName);

            return mediaFileInfo != null ? Selector(mediaFileInfo) : null;
        }

        public IEnumerable<MediaLibraryFileDto> GetMediaLibraryDtos(params Guid[] fileGuids) =>
            GetBaseQuery((baseQuery) =>
                baseQuery.WhereIn("FileGUID", fileGuids));

        public IEnumerable<MediaLibraryFileDto> GetMediaLibraryDtos(params string[] extensions) =>
            GetBaseQuery((baseQuery) =>
                baseQuery.WhereIn("FileExtension", extensions));

        protected IEnumerable<MediaLibraryFileDto> GetBaseQuery(Func<ObjectQuery<MediaFileInfo>, ObjectQuery<MediaFileInfo>> filter)
        {
            var baseQuery = MediaFileInfoProvider.GetMediaFiles()
                .WhereEquals("FileLibraryID", MediaLibraryId);

            return filter(baseQuery).Select(file => Selector(file));
        }

        protected MediaLibraryFileDto Selector(MediaFileInfo mediaFileInfo) =>
            new MediaLibraryFileDto()
            {
                Guid = mediaFileInfo.FileGUID,
                Title = mediaFileInfo.FileTitle,
                Extension = mediaFileInfo.FileExtension,
                DirectUrl = MediaLibraryHelper.GetDirectUrl(mediaFileInfo),
                PermanentUrl = MediaLibraryHelper.GetPermanentUrl(mediaFileInfo),
                Width = mediaFileInfo.FileImageWidth,
                Height = mediaFileInfo.FileImageHeight
            };
    }
}
