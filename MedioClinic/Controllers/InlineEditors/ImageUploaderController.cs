using System;
using System.Web;
using System.Web.Mvc;

using MedioClinic.Utils;

namespace MedioClinic.Controllers.Widgets
{
    public class ImageUploaderController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\ImageUploaderEditor";

        protected IFileManager FileManagementHelper { get; }

        protected IErrorHelper ErrorHelper { get; }

        public ImageUploaderController(IFileManager fileManagementHelper, IErrorHelper errorHandler)
        {
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
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
                    imageGuid = FileManagementHelper.AddMediaLibraryFile(file, TempPath, libraryName: mediaLibraryName, librarySiteName: mediaLibrarySiteName);
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