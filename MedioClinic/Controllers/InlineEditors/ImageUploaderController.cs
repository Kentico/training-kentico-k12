using System;
using System.Web;
using System.Web.Mvc;

using MedioClinic.Utils;

namespace MedioClinic.Controllers.Widgets
{
    public class ImageUploaderController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\ImageUploaderEditor";

        protected IFileManagementHelper FileManagementHelper { get; }

        protected IErrorHandler ErrorHandler { get; }

        public ImageUploaderController(IFileManagementHelper fileManagementHelper, IErrorHandler errorHandler)
        {
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
            ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        // POST: ImageUploader/Upload
        [HttpPost]
        public ActionResult Upload(int pageId)
        {
            ErrorHandler.CheckEditMode(HttpContext, nameof(ImageUploaderController.Upload));
            var page = FileManagementHelper.GetPage(pageId);
            var imageGuid = Guid.Empty;

            if (Request.Files[0] is HttpPostedFileWrapper file && file != null)
            {
                try
                {
                    imageGuid = FileManagementHelper.AddUnsortedAttachment(page, TempPath, file);
                }
                catch (Exception ex)
                {
                    return ErrorHandler.HandleException(nameof(ImageUploaderController.Upload), ex, ErrorHandler.UnprocessableStatusCode);
                }

                return Json(new { guid = imageGuid }); 
            }

            return new HttpStatusCodeResult(ErrorHandler.UnprocessableStatusCode);
        }
    }
}