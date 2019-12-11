using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.MediaLibrary;
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
                _mediaLibraryId = value;
            }
        }

        public int? MediaLibrarySiteId
        {
            get => _mediaLibrarySiteId == null && !string.IsNullOrEmpty(_mediaLibrarySiteName)
                ? SiteInfoProvider.GetSiteID(_mediaLibrarySiteName)
                : _mediaLibrarySiteId;

            set
            {
                _mediaLibrarySiteId = value;
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
                _mediaLibraryName = value;
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
                _mediaLibrarySiteName = value;
            }
        }

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
