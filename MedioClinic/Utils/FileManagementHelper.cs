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

            if (page != null && !CheckPagePermissions(page))
            {
                throw new HttpException(403, "You are not authorized to upload an image to the page.");
            }

            return page;
        }

        public bool CheckPagePermissions(TreeNode page) => 
            page?.CheckPermissions(PermissionsEnum.Modify, SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser) ?? false;

        public string EnsureUploadDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Directory path must be specified.", nameof(directoryPath));
            }

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }

        public string GetTempFilePath(string directoryPath, string fileName)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Directory path must be specified.", nameof(directoryPath));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("Cannot upload file without file name.");
            }

            if (!AllowedExtensions.Contains(Path.GetExtension(fileName)))
            {
                throw new InvalidOperationException("Cannot upload file of this type.");
            }

            return Path.Combine(directoryPath, fileName);
        }

        public Guid AddUnsortedAttachment(TreeNode page, string uploadDirectory, HttpPostedFileWrapper file)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            if (string.IsNullOrEmpty(uploadDirectory))
            {
                throw new ArgumentException("Upload directory path must be specified.", nameof(uploadDirectory));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var directoryPath = EnsureUploadDirectory(uploadDirectory);
            var imagePath = GetTempFilePath(directoryPath, file.FileName);
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