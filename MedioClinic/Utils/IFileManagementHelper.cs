using CMS.DocumentEngine;
using System;
using System.Web;

namespace MedioClinic.Utils
{
    public interface IFileManagementHelper
    {
        TreeNode GetPage(int pageId);
        string EnsureUploadDirectory(string directoryPath);
        bool CheckPagePermissions(TreeNode page);
        string GetTempFilePath(string directoryPath, HttpPostedFileBase file);
        Guid AddUnsortedAttachment(TreeNode page, string requestFileName, HttpRequestBase request, string uploadDirectory);
    }
}