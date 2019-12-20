using System;
using System.Web;

namespace MedioClinic.Utils
{
    /// <summary>
    /// Handles filesystem-related tasks.
    /// </summary>
    public interface IFileManager
    {
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
        Guid AddMediaLibraryFile(HttpPostedFileWrapper file, string uploadDirectory, string libraryName, string librarySiteName, string libraryFolderPath = null, bool checkPermisions = false);

        /// <summary>
        /// Adds a new file to a given media library.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="uploadDirectory">Physical upload directory.</param>
        /// <param name="mediaLibraryId">Library ID.</param>
        /// <param name="libraryFolderPath">Library folder to save to.</param>
        /// <returns>The GUID of the file in the media library.</returns>
        Guid AddMediaLibraryFile(HttpPostedFileWrapper file, string uploadDirectory, int mediaLibraryId, string libraryFolderPath = null, bool checkPermisions = false);
        
        /// <summary>
        /// Checks for the existence of a directory, and creates it if necessary.
        /// </summary>
        /// <param name="directoryPath">Path of the directory.</param>
        void EnsureDirectory(string directoryPath);
        
        /// <summary>
        /// Makes sure that a local file exists.
        /// </summary>
        /// <param name="physicalPath">File physical path.</param>
        /// <param name="fileBinary">File byte array.</param>
        /// <param name="forceOverwrite">Flag to overwrite an existing file.</param>
        void EnsureFile(string physicalPath, byte[] fileBinary, bool forceOverwrite = false);

        /// <summary>
        /// Converts a physical into a server-relative path.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        /// <param name="physicalPath">Physical path.</param>
        /// <returns>A server-relative path.</returns>
        string GetServerRelativePath(HttpRequestBase request, string physicalPath);

        /// <summary>
        /// Gets a byte array of a posted file.
        /// </summary>
        /// <param name="file">The posted file.</param>
        /// <returns>The byte array of the file.</returns>
        byte[] GetPostedFileBinary(HttpPostedFileBase file);
    }
}