using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MedioClinic.Utils
{
    public class FileManagementHelper : IFileManagementHelper
    {
        public static HashSet<string> AllowedExtensions =>
            new HashSet<string>(new[]
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

        public TreeNode GetPage(int pageId)
        {
            var page = DocumentHelper.GetDocument(pageId, null);

            if (!CheckPagePermissions(page))
            {
                throw new HttpException(403, "You are not authorized to upload an image to the page.");
            }

            return page;
        }

        public bool CheckPagePermissions(TreeNode page) => 
            page?.CheckPermissions(PermissionsEnum.Modify, SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser) ?? false;

        public string EnsureUploadDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }

        public string GetTempFilePath(string directoryPath, HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("Cannot upload file without file name.");
            }

            if (!AllowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new InvalidOperationException("Cannot upload file of this type.");
            }

            return Path.Combine(directoryPath, fileName);
        }

        public Guid AddUnsortedAttachment(TreeNode page, string requestFileName, HttpRequestBase request, string uploadDirectory)
        {
            if (!(request.Files[requestFileName] is HttpPostedFileWrapper file))
            {
                return Guid.Empty;
            }

            var directoryPath = EnsureUploadDirectory(uploadDirectory);
            var imagePath = GetTempFilePath(directoryPath, file);
            byte[] data = new byte[file.ContentLength];
            file.InputStream.Seek(0, SeekOrigin.Begin);
            file.InputStream.Read(data, 0, file.ContentLength);
            CMS.IO.File.WriteAllBytes(imagePath, data);
            var attachmentGuid = DocumentHelper.AddUnsortedAttachment(page, Guid.Empty, imagePath).AttachmentGUID;
            CMS.IO.File.Delete(imagePath);

            return attachmentGuid;
        }
    }
}