using Business.Services.Context;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MedioClinic.Extensions;
using MedioClinic.Utils;

namespace MedioClinic.Controllers
{
    public class MediaLibraryUploaderController : Controller
    {
        protected const int ErrorStatusCode = 422;

        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\MediaLibraryUploader";

        protected ISiteContextService SiteContextService { get; }

        protected IFileManagementHelper FileManagementHelper { get; }

        public MediaLibraryUploaderController(ISiteContextService siteContextService, IFileManagementHelper fileManagementHelper)
        {
            SiteContextService = siteContextService ?? throw new ArgumentNullException(nameof(siteContextService));
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
        }

        // POST: MediaLibraryUploader/Upload
        [HttpPost]
        public ActionResult Upload(int pageId, string filePathId, int mediaLibraryId)
        {
            var page = FileManagementHelper.GetPage(pageId);
            var pathSegments = page.NodeAliasPath.Split('/');

            if ((Request.Files[0] is HttpPostedFileWrapper file))
            {
                var directoryPath = FileManagementHelper.EnsureUploadDirectory(TempPath);
                var imagePath = FileManagementHelper.GetTempFilePath(directoryPath, file);

                try
                {
                    MediaLibraryInfoProvider.CreateMediaLibraryFolder(SiteContextService.SiteName, mediaLibraryId, page.NodeAliasPath);
                }
                catch (Exception ex)
                {
                    // TODO Log exception
                    // return 500?
                }

                byte[] data = new byte[file.ContentLength];
                file.InputStream.Seek(0, SeekOrigin.Begin);
                file.InputStream.Read(data, 0, file.ContentLength);
                CMS.IO.File.WriteAllBytes(imagePath, data);
                CMS.IO.FileInfo fileInfo = CMS.IO.FileInfo.New(imagePath);

                if (fileInfo != null)
                {
                    MediaFileInfo mediaFile = new MediaFileInfo(imagePath, mediaLibraryId);
                    var mediaFilePathSegments = pathSegments.GetArrayRange(1);
                    var mediaFilePath = $"{mediaFilePathSegments.Join("/")}/";
                    mediaFile.FileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
                    mediaFile.FilePath = mediaFilePath;
                    mediaFile.FileExtension = fileInfo.Extension;
                    mediaFile.FileMimeType = MimeTypeHelper.GetMimetype(fileInfo.Extension);
                    mediaFile.FileSiteID = SiteContext.CurrentSiteID;
                    mediaFile.FileLibraryID = mediaLibraryId;
                    mediaFile.FileSize = fileInfo.Length;

                    try
                    {
                        MediaFileInfoProvider.SetMediaFileInfo(mediaFile);
                    }
                    catch (Exception ex)
                    {
                        return new HttpStatusCodeResult(ErrorStatusCode);
                    }

                    try
                    {
                        CMS.IO.File.Delete(imagePath);
                    }
                    catch (Exception ex)
                    {
                        // TODO Log exception
                    }

                    return Json(new
                    {
                        filePathId,
                        fileGuid = mediaFile.FileGUID.ToString()
                    });
                }
            }

            return new HttpStatusCodeResult(ErrorStatusCode);
        }
    }
}