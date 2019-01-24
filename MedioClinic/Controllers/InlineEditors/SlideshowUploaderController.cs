using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic.Controllers
{
    public class SlideshowUploaderController : Controller
    {
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

        // POST: SlideshowUploader/Upload
        [HttpPost]
        public JsonResult Upload(int pageId)
        {
            //var page = DocumentHelper.GetDocument(pageId, null);

            ////foreach (var fileName in Request.Files)
            ////{

            ////}

            //return Json(new { guid = Guid.NewGuid() });

            if (!HttpContext.Kentico().PageBuilder().EditMode)
            {
                throw new HttpException(403, "It is allowed to upload an image only when the page builder is in the edit mode.");
            }

            var page = DocumentHelper.GetDocument(pageId, null);
            if (!CheckPagePermissions(page))
            {
                throw new HttpException(403, "You are not authorized to upload an image to the page.");
            }

            var imageGuid = Guid.Empty;

            foreach (string requestFileName in Request.Files)
            {
                imageGuid = AddUnsortedAttachment(page, requestFileName);
            }

            return Json(new { guid = imageGuid });
        }

        private Guid AddUnsortedAttachment(TreeNode page, string requestFileName)
        {
            if (!(Request.Files[requestFileName] is HttpPostedFileWrapper file))
            {
                return Guid.Empty;
            }

            var directoryPath = EnsureUploadDirectory();
            var imagePath = GetTempFilePath(directoryPath, file);

            byte[] data = new byte[file.ContentLength];
            file.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
            file.InputStream.Read(data, 0, file.ContentLength);

            CMS.IO.File.WriteAllBytes(imagePath, data);

            var attachmentGuid = DocumentHelper.AddUnsortedAttachment(page, Guid.Empty, imagePath).AttachmentGUID;

            CMS.IO.File.Delete(imagePath);

            return attachmentGuid;
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


        private string EnsureUploadDirectory()
        {
            var directoryPath = $"{Server.MapPath(@"~\")}App_Data\\Temp\\ImageUploader";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }


        private bool CheckPagePermissions(TreeNode page)
        {
            return page?.CheckPermissions(PermissionsEnum.Modify, SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser) ?? false;
        }
    }
}