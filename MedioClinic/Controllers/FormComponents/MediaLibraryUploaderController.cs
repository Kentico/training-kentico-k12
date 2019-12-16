using System;
using System.Web;
using System.Web.Mvc;

using MedioClinic.Utils;

namespace MedioClinic.Controllers
{
    public class MediaLibraryUploaderController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\MediaLibraryUploader";

        protected IFileManager FileManager { get; }

        protected IErrorHelper ErrorHelper { get; }

        public MediaLibraryUploaderController(IFileManager fileManager, IErrorHelper errorHandler)
        {
            FileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            ErrorHelper = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        // POST: MediaLibraryUploader/Upload
        [HttpPost]
        public ActionResult Upload(string fileInputElementId, int mediaLibraryId)
        {
            if (Request.Files[0] is HttpPostedFileWrapper file && file != null)
            {
                Guid fileGuid;

                try
                {
                    fileGuid = FileManager.AddMediaLibraryFile(file, TempPath, mediaLibraryId);
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleException(nameof(MediaLibraryUploaderController), nameof(Upload), ex);
                }

                return Json(new
                {
                    fileInputElementId,
                    fileGuid
                });
            }

            return new HttpStatusCodeResult(ErrorHelper.UnprocessableStatusCode);
        }
    }
}