using System;
using System.IO;
using System.Web;

using Business.Repository.MediaLibrary;
using MedioClinic.Config;

namespace MedioClinic.Utils
{
    public class FileManager : IFileManager
    {
        protected IErrorHelper ErrorHelper { get; }

        protected IMediaLibraryRepository MediaLibraryRepository { get; }

        public FileManager(IErrorHelper errorHelper, IMediaLibraryRepository mediaLibraryRepository)
        {
            ErrorHelper = errorHelper ?? throw new ArgumentNullException(nameof(errorHelper));
            MediaLibraryRepository = mediaLibraryRepository ?? throw new ArgumentNullException(nameof(mediaLibraryRepository));
        }

        protected static string GetSuffixedFileName(string fileName, string fileExtension, int currentSuffix) =>
            currentSuffix == 0 ? fileName : $"{fileName} ({currentSuffix}).{fileExtension}";

        public string EnsureUploadDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Directory path was not specified.", nameof(directoryPath));
            }

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }

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

        public Guid AddMediaLibraryFile(HttpPostedFileWrapper file, string uploadDirectory, string libraryName, string librarySiteName, string libraryFolderPath = null)
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

            return AddMediaLibraryFileInternal(file, uploadDirectory, libraryFolderPath);
        }

        public Guid AddMediaLibraryFile(HttpPostedFileWrapper file, string uploadDirectory, int mediaLibraryId, string libraryFolderPath = null)
        {
            MediaLibraryRepository.MediaLibraryId = mediaLibraryId;

            return AddMediaLibraryFileInternal(file, uploadDirectory, libraryFolderPath);
        }

        protected Guid AddMediaLibraryFileInternal(HttpPostedFileWrapper file, string uploadDirectory, string libraryFolderPath = null)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!AppConfig.AllowedImageExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new InvalidOperationException("The app is not configured to allow this type of image.");
            }

            var directory = EnsureUploadDirectory(uploadDirectory);
            var (name, extension) = GetNameAndExtension(file.FileName);
            var uploadFilePath = GetNonCollidingFilePath(directory, name, extension);
            file.SaveAs(uploadFilePath);

            try
            {
                return MediaLibraryRepository.AddMediaLibraryFile(uploadFilePath, libraryFolderPath);
            }
            finally
            {
                CMS.IO.File.Delete(uploadFilePath);
            }
        }

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