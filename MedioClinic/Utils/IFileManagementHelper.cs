using System;
using System.Web;

using CMS.DocumentEngine;

namespace MedioClinic.Utils
{
    public interface IFileManagementHelper
    {
        /// <summary>
        /// Gets the Kentico page, based on its ID.
        /// </summary>
        /// <param name="pageId">The ID of the page.</param>
        /// <returns>The Kentico page.</returns>
        TreeNode GetPage(int pageId);

        /// <summary>
        /// Checks for the existence of an upload directory, and, creates it eventually.
        /// </summary>
        /// <param name="directoryPath">Path of the upload directory.</param>
        /// <returns>Path of the upload directory.</returns>
        string EnsureUploadDirectory(string directoryPath);

        /// <summary>
        /// Checks if the current authenticated user is allowed to modify a given Kentico page.
        /// </summary>
        /// <param name="page">The Kentico page.</param>
        /// <returns><see langword="true" /> if the user has the permission, otherwise <see langword="false"/>.</returns>
        bool CheckPagePermissions(TreeNode page);

        /// <summary>
        /// Gets the complete local filesystem path to a file.
        /// </summary>
        /// <param name="directoryPath">Directory path.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>The complete path.</returns>
        string GetTempFilePath(string directoryPath, string fileName);

        /// <summary>
        /// Adds an unsorted attachment to a given Kentico page.
        /// </summary>
        /// <param name="page">The Kentico page.</param>
        /// <param name="uploadDirectory">The upload directory.</param>
        /// <param name="file">The file to be added.</param>
        /// <returns>The <see cref="Guid"/> of the page attachment.</returns>
        Guid AddUnsortedAttachment(TreeNode page, string uploadDirectory, HttpPostedFileWrapper file);
    }
}