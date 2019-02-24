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

        protected IErrorHelper ErrorHelper { get; }

        public ImageUploaderController(IFileManagementHelper fileManagementHelper, IErrorHelper errorHandler)
        {
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
            ErrorHelper = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        // POST: ImageUploader/Upload/[pageId]
        [HttpPost]
        public ActionResult Upload(int pageId)
        {
            ErrorHelper.CheckEditMode(HttpContext, nameof(ImageUploaderController.Upload));
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
                    return ErrorHelper.HandleException(nameof(ImageUploaderController.Upload), ex, ErrorHelper.UnprocessableStatusCode);
                }

                return Json(new { guid = imageGuid }); 
            }

            return new HttpStatusCodeResult(ErrorHelper.UnprocessableStatusCode);
        }
    }
}