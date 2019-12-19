using System;
using System.Web;
using System.Web.Mvc;

using MedioClinic.Utils;

namespace MedioClinic.Controllers.Widgets
{
    public class ImageUploaderController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\ImageUploaderEditor";

        protected IFileManager FileManager { get; }

        protected IErrorHelper ErrorHelper { get; }

        public ImageUploaderController(IFileManager fileManager, IErrorHelper errorHandler)
        {
            FileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            ErrorHelper = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        // POST: ImageUploader/Upload/[mediaLibraryName, mediaLibrarySiteName]
        [HttpPost]
        public ActionResult Upload(string mediaLibraryName, string mediaLibrarySiteName)
        {
            ErrorHelper.CheckEditMode(HttpContext, nameof(ImageUploaderController.Upload));
            var imageGuid = Guid.Empty;

            if (Request.Files[0] is HttpPostedFileWrapper file)
            {
                try
                {
                    imageGuid = FileManager.AddMediaLibraryFile(file, TempPath, libraryName: mediaLibraryName, librarySiteName: mediaLibrarySiteName);
                }
                catch (Exception ex)
                {
                    var result = ErrorHelper.HandleException(
                        nameof(ImageUploaderController), 
                        nameof(Upload), 
                        ex, 
                        ErrorHelper.UnprocessableStatusCode);

                    return result;
                }

                return Json(new { guid = imageGuid }); 
            }

            return new HttpStatusCodeResult(ErrorHelper.UnprocessableStatusCode);
        }
    }
}