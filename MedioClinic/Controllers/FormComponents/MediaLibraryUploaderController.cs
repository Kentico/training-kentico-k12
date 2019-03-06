using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using MedioClinic.Utils;

namespace MedioClinic.Controllers
{
    public class MediaLibraryUploaderController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\MediaLibraryUploader";

        protected IFileManagementHelper FileManagementHelper { get; }

        protected IErrorHelper ErrorHelper { get; }

        public MediaLibraryUploaderController(
            IFileManagementHelper fileManagementHelper, 
            IErrorHelper errorHandler)
        {
            FileManagementHelper = fileManagementHelper 
                ?? throw new ArgumentNullException(nameof(fileManagementHelper));
            ErrorHelper = errorHandler 
                ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        // POST: MediaLibraryUploader/Upload
        [HttpPost]
        public ActionResult Upload(string filePathId, int mediaLibraryId)
        {
            if (Request.Files[0] is HttpPostedFileWrapper file && file != null)
            {
                string directoryPath = null;

                try
                {
                    directoryPath = FileManagementHelper.EnsureUploadDirectory(TempPath);
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleException(nameof(MediaLibraryUploaderController.Upload), ex);
                }

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    string imagePath = null;

                    try
                    {
                        imagePath = FileManagementHelper.GetTempFilePath(directoryPath, file.FileName);
                    }
                    catch (Exception ex)
                    {
                        return ErrorHelper.HandleException(nameof(MediaLibraryUploaderController.Upload), ex);
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
                            return ErrorHelper.HandleException(
                                nameof(MediaLibraryUploaderController.Upload), 
                                ex, 
                                ErrorHelper.UnprocessableStatusCode);
                        }

                        if (fileInfo != null)
                        {
                            return CreateMediaFile(filePathId, mediaLibraryId, imagePath, fileInfo);
                        }
                    }
                }
            }

            return new HttpStatusCodeResult(ErrorHelper.UnprocessableStatusCode);
        }

        /// <summary>
        /// Creates a media file and handles possible errors.
        /// </summary>
        /// <param name="filePathId">ID of the file path HTML element.</param>
        /// <param name="mediaLibraryId">ID of the media library.</param>
        /// <param name="imagePath">Local path to the image.</param>
        /// <param name="fileInfo">File information.</param>
        /// <returns>Either a JSON response, or an HTTP status code.</returns>
        protected ActionResult CreateMediaFile(string filePathId, int mediaLibraryId, string imagePath, CMS.IO.FileInfo fileInfo)
        {
            MediaFileInfo mediaFileInfo = null;

            try
            {
                mediaFileInfo = CreateMediafileInfo(mediaLibraryId, fileInfo);
            }
            catch (Exception ex)
            {
                return ErrorHelper.HandleException(
                    nameof(MediaLibraryUploaderController.Upload),
                    ex,
                    ErrorHelper.UnprocessableStatusCode);
            }

            try
            {
                CMS.IO.File.Delete(imagePath);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(nameof(MediaLibraryUploaderController.Upload), ex);
            }

            return Json(new
            {
                filePathId,
                fileGuid = mediaFileInfo.FileGUID.ToString()
            });
        }

        /// <summary>
        /// Gets details of an uploaded file.
        /// </summary>
        /// <param name="file">Uploaded file contents.</param>
        /// <param name="filePath">Path to store the file on the disk.</param>
        /// <returns>The file details.</returns>
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
        /// <param name="fileInfo">File info.</param>
        /// <returns>Media file information.</returns>
        protected MediaFileInfo CreateMediafileInfo(
            int mediaLibraryId, CMS.IO.FileInfo fileInfo)
        {
            MediaFileInfo mediaFile = new MediaFileInfo(fileInfo?.FullName, mediaLibraryId);
            mediaFile.FileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
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