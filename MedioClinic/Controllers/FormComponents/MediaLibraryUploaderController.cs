using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using Business.Services.Context;
using MedioClinic.Extensions;
using MedioClinic.Utils;

namespace MedioClinic.Controllers
{
    public class MediaLibraryUploaderController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\MediaLibraryUploader";

        protected ISiteContextService SiteContextService { get; }

        protected IFileManagementHelper FileManagementHelper { get; }

        protected IErrorHandler ErrorHandler { get; }

        public MediaLibraryUploaderController(ISiteContextService siteContextService, IFileManagementHelper fileManagementHelper, IErrorHandler errorHandler)
        {
            SiteContextService = siteContextService ?? throw new ArgumentNullException(nameof(siteContextService));
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
            ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        // POST: MediaLibraryUploader/Upload
        [HttpPost]
        public ActionResult Upload(int pageId, string filePathId, int mediaLibraryId)
        {
            var page = FileManagementHelper.GetPage(pageId);

            if (Request.Files[0] is HttpPostedFileWrapper file && file != null)
            {
                string directoryPath = null;

                try
                {
                    directoryPath = FileManagementHelper.EnsureUploadDirectory(TempPath);
                }
                catch (Exception ex)
                {
                    return ErrorHandler.HandleException(nameof(MediaLibraryUploaderController.Upload), ex);
                }

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    string imagePath = null;

                    try
                    {
                        imagePath = FileManagementHelper.GetTempFilePath(directoryPath, file.FileName);
                        MediaLibraryInfoProvider.CreateMediaLibraryFolder(SiteContextService.SiteName, mediaLibraryId, page.NodeAliasPath);
                    }
                    catch (Exception ex)
                    {
                        return ErrorHandler.HandleException(nameof(MediaLibraryUploaderController.Upload), ex);
                    }

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        CMS.IO.FileInfo fileInfo = null;

                        try
                        {
                            fileInfo = GetFileInfo(file, imagePath);
                        }
                        catch (Exception ex)
                        {
                            return ErrorHandler.HandleException(nameof(MediaLibraryUploaderController.Upload), ex, ErrorHandler.UnprocessableStatusCode);
                        }

                        if (fileInfo != null)
                        {
                            MediaFileInfo mediaFile = null;

                            try
                            {
                                mediaFile = CreateMediafile(mediaLibraryId, page.NodeAliasPath, imagePath, fileInfo);
                            }
                            catch (Exception ex)
                            {
                                return ErrorHandler.HandleException(nameof(MediaLibraryUploaderController.Upload), ex, ErrorHandler.UnprocessableStatusCode);
                            }

                            try
                            {
                                CMS.IO.File.Delete(imagePath);
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.LogException(nameof(MediaLibraryUploaderController.Upload), ex);
                            }

                            return Json(new
                            {
                                filePathId,
                                fileGuid = mediaFile.FileGUID.ToString()
                            });
                        }
                    }
                }
            }

            return new HttpStatusCodeResult(ErrorHandler.UnprocessableStatusCode);
        }

        /// <summary>
        /// Gets details of an uploaded file.
        /// </summary>
        /// <param name="file">Uploaded file contents.</param>
        /// <param name="filePath">Path to store the file on the disk.</param>
        /// <returns></returns>
        protected CMS.IO.FileInfo GetFileInfo(HttpPostedFileWrapper file, string filePath)
        {
            byte[] data = new byte[file.ContentLength];
            file.InputStream.Seek(0, SeekOrigin.Begin);
            file.InputStream.Read(data, 0, file.ContentLength);
            CMS.IO.File.WriteAllBytes(filePath, data);
            CMS.IO.FileInfo fileInfo = CMS.IO.FileInfo.New(filePath);

            return fileInfo;
        }

        /// <summary>
        /// Creates a new file in the Kentico media library.
        /// </summary>
        /// <param name="mediaLibraryId">Media library ID.</param>
        /// <param name="nodeAliasPath">Node alias path to replicate in the media library.</param>
        /// <param name="filePath">Path to the physical file on the disk.</param>
        /// <param name="fileInfo">File info.</param>
        /// <returns></returns>
        protected MediaFileInfo CreateMediafile(int mediaLibraryId, string nodeAliasPath, string filePath, CMS.IO.FileInfo fileInfo)
        {
            MediaFileInfo mediaFile = new MediaFileInfo(filePath, mediaLibraryId);
            var nodeAliasPathSegments = nodeAliasPath.Split('/');
            var mediaFilePathSegments = nodeAliasPathSegments.GetArrayRange(1);
            var mediaFilePath = $"{mediaFilePathSegments.Join("/")}/";
            mediaFile.FileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
            mediaFile.FilePath = mediaFilePath;
            mediaFile.FileExtension = fileInfo.Extension;
            mediaFile.FileMimeType = MimeTypeHelper.GetMimetype(fileInfo.Extension);
            mediaFile.FileSiteID = SiteContext.CurrentSiteID;
            mediaFile.FileLibraryID = mediaLibraryId;
            mediaFile.FileSize = fileInfo.Length;
            MediaFileInfoProvider.SetMediaFileInfo(mediaFile);

            return mediaFile;
        }
    }
}