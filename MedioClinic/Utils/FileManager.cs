using System;
using System.IO;
using System.Web;

using Business.Repository.MediaLibrary;
using MedioClinic.Config;

namespace MedioClinic.Utils
{
    public class FileManager : IFileManager
    {
        protected IMediaLibraryRepository MediaLibraryRepository { get; }

        public FileManager(IMediaLibraryRepository mediaLibraryRepository)
        {
            MediaLibraryRepository = mediaLibraryRepository ?? throw new ArgumentNullException(nameof(mediaLibraryRepository));
        }

        // Builders
        protected static string GetSuffixedFileName(string fileName, string fileExtension, int currentSuffix) =>
            currentSuffix == 0 ? fileName : $"{fileName} ({currentSuffix}).{fileExtension}";

        // Builders
        public string GetFilePath(string directoryPath, string fileName)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Directory path was not specified.", nameof(directoryPath));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("Cannot upload file without file name.");
            }

            return Path.Combine(directoryPath, fileName);
        }

        // Builders
        public Guid AddMediaLibraryFile(
            HttpPostedFileWrapper file, 
            string uploadDirectory, 
            string libraryName, 
            string librarySiteName, 
            string libraryFolderPath = null, 
            bool checkPermisions = false)
        {
            if (string.IsNullOrEmpty(libraryName))
            {
                throw new ArgumentException("Media library name is not specified.", nameof(libraryName));
            }

            if (string.IsNullOrEmpty(librarySiteName))
            {
                throw new ArgumentException("Media library site name is not specified.", nameof(librarySiteName));
            }

            MediaLibraryRepository.MediaLibraryName = libraryName;
            MediaLibraryRepository.MediaLibrarySiteName = librarySiteName;

            return AddMediaLibraryFileInternal(file, uploadDirectory, libraryFolderPath, checkPermisions);
        }

        // Builders
        public Guid AddMediaLibraryFile(
            HttpPostedFileWrapper file, 
            string uploadDirectory, 
            int mediaLibraryId, 
            string libraryFolderPath = null, 
            bool checkPermisions = false)
        {
            MediaLibraryRepository.MediaLibraryId = mediaLibraryId;

            return AddMediaLibraryFileInternal(file, uploadDirectory, libraryFolderPath, checkPermisions);
        }

        // Builders, Identity
        public void EnsureDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Directory path was not specified.", nameof(directoryPath));
            }

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        // Identity
        public void EnsureFile(string physicalPath, byte[] fileBinary, bool forceOverwrite = false)
        {
            if (string.IsNullOrEmpty(physicalPath))
            {
                throw new ArgumentException("Physical path was not specified.", nameof(physicalPath));
            }

            if (!File.Exists(physicalPath) || forceOverwrite)
            {
                File.WriteAllBytes(physicalPath, fileBinary);
            }
        }

        // Identity
        public string GetServerRelativePath(HttpRequestBase request, string physicalPath)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(physicalPath))
            {
                throw new ArgumentException("Physical path was not specified.", nameof(physicalPath));
            }

            var trimmed = physicalPath.Substring(request.PhysicalApplicationPath.Length);

            return $"~/{trimmed.Replace('\\', '/')}";
        }

        // Identity
        public byte[] GetPostedFileBinary(HttpPostedFileBase file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            byte[] data = new byte[file.ContentLength];
            file.InputStream.Seek(0, SeekOrigin.Begin);
            file.InputStream.Read(data, 0, file.ContentLength);

            return data;
        }

        // Builders
        protected Guid AddMediaLibraryFileInternal(HttpPostedFileWrapper file, string uploadDirectory, string libraryFolderPath = null, bool checkPermisions = false)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!AppConfig.AllowedImageExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new InvalidOperationException("The app is not configured to allow this type of image.");
            }

            EnsureDirectory(uploadDirectory);
            var (name, extension) = GetNameAndExtension(file.FileName);
            var uploadFilePath = GetNonCollidingFilePath(uploadDirectory, name, extension);
            file.SaveAs(uploadFilePath);

            try
            {
                return MediaLibraryRepository.AddMediaLibraryFile(uploadFilePath, libraryFolderPath, checkPermisions);
            }
            finally
            {
                CMS.IO.File.Delete(uploadFilePath);
            }
        }

        // Builders
        protected (string Name, string Extension) GetNameAndExtension(string completeFileName)
        {
            if (string.IsNullOrEmpty(completeFileName))
            {
                throw new ArgumentException("File name is null or an empty string.", nameof(completeFileName));
            }

            var separator = '.';
            var segments = completeFileName.Split(separator);

            if (segments?.Length > 1)
            {
                var subtractedLength = segments.Length - 1;
                string[] segmentsExceptLast = new string[subtractedLength];
                Array.Copy(segments, segmentsExceptLast, subtractedLength);
                var name = segmentsExceptLast.Length == 1 ? segmentsExceptLast[0] : string.Join(separator.ToString(), segmentsExceptLast);

                return (name, segments[subtractedLength]);
            }
            else
            {
                return (completeFileName, null);
            }
        }

        // Builders
        protected string GetNonCollidingFilePath(string directoryPath, string fileName, string fileExtension, int currentSuffix = 0)
        {
            string newFileName = GetSuffixedFileName(fileName, fileExtension, currentSuffix);
            var filePath = GetFilePath(directoryPath, $"{newFileName}.{fileExtension}");

            return File.Exists(filePath)
                ? GetNonCollidingFilePath(directoryPath, fileName, fileExtension, currentSuffix + 1)
                : filePath;
        }
    }
}