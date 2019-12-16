using System;
using System.Web;

namespace MedioClinic.Utils
{
    public interface IFileManager
    {
        /// <summary>
        /// Checks for the existence of an upload directory, and, creates it eventually.
        /// </summary>
        /// <param name="directoryPath">Path of the upload directory.</param>
        /// <returns>Path of the upload directory.</returns>
        string EnsureUploadDirectory(string directoryPath);

        /// <summary>
        /// Gets the complete local filesystem path to a file.
        /// </summary>
        /// <param name="directoryPath">Directory path.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>The complete path.</returns>
        string GetFilePath(string directoryPath, string fileName);

        /// <summary>
        /// Adds a new file to a given media library.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="uploadDirectory">Physical upload directory.</param>
        /// <param name="libraryName">Library code name.</param>
        /// <param name="librarySiteName">Library site code name.</param>
        /// <param name="libraryFolderPath">Library folder to save to.</param>
        /// <returns>The GUID of the file in the media library.</returns>
        Guid AddMediaLibraryFile(HttpPostedFileWrapper file, string uploadDirectory, string libraryName, string librarySiteName, string libraryFolderPath = null);

        /// <summary>
        /// Adds a new file to a given media library.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="uploadDirectory">Physical upload directory.</param>
        /// <param name="mediaLibraryId">Library ID.</param>
        /// <param name="libraryFolderPath">Library folder to save to.</param>
        /// <returns>The GUID of the file in the media library.</returns>
        Guid AddMediaLibraryFile(HttpPostedFileWrapper file, string uploadDirectory, int mediaLibraryId, string libraryFolderPath = null);
    }
}