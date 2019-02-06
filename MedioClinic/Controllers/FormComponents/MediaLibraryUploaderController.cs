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

namespace MedioClinic.Controllers
{
    public class MediaLibraryUploaderController : Controller
    {
        protected const int ErrorStatusCode = 422;

        protected ISiteContextService SiteContextService { get; }

        public MediaLibraryUploaderController(ISiteContextService siteContextService)
        {
            SiteContextService = siteContextService;
        }

        private static readonly HashSet<string> allowedExtensions = new HashSet<string>(new[]
        {
            ".bmp",
            ".gif",
            ".ico",
            ".png",
            ".wmf",
            ".jpg",
            ".jpeg",
            ".tiff",
            ".tif"
        }, StringComparer.OrdinalIgnoreCase);

        private TreeNode GetPageWithSanityChecks(int pageId)
        {
            var page = DocumentHelper.GetDocument(pageId, null);

            if (!CheckPagePermissions(page))
            {
                throw new HttpException(403, "You are not authorized to upload an image to the page.");
            }

            return page;
        }

        private bool CheckPagePermissions(TreeNode page)
        {
            return page?.CheckPermissions(PermissionsEnum.Modify, SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser) ?? false;
        }

        // POST: MediaLibraryUploader/Upload
        [HttpPost]
        public ActionResult Upload(int pageId, string filePathId, int mediaLibraryId)
        {
            var page = GetPageWithSanityChecks(pageId);
            var pathSegments = page.NodeAliasPath.Split('/');

            if ((Request.Files[0] is HttpPostedFileWrapper file))
            {
                var directoryPath = EnsureUploadDirectory(DirectoryPath);
                var imagePath = GetTempFilePath(directoryPath, file);

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

        string DirectoryPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\MediaLibraryUploader";

        private string EnsureUploadDirectory(string directoryPath)
        {
            //var directoryPath = $"{Server.MapPath(@"~\")}App_Data\\Temp\\MediaLibraryUploader";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }

        private string GetTempFilePath(string directoryPath, HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("Cannot upload file without file name.");
            }

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new InvalidOperationException("Cannot upload file of this type.");
            }

            return Path.Combine(directoryPath, fileName);
        }
    }
}