using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine;
using MedioClinic.Utils;

namespace MedioClinic.Controllers
{
    public class SlideshowManagementController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\SlideshowEditor";

        protected IFileManagementHelper FileManagementHelper { get; }

        protected IErrorHandler ErrorHandler { get; }

        public SlideshowManagementController(IFileManagementHelper fileManagementHelper, IErrorHandler errorHandler)
        {
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
            ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(ErrorHandler));
        }

        // POST: SlideshowManagement/Upload
        [HttpPost]
        public ActionResult Upload(int pageId)
        {
            ErrorHandler.CheckEditMode(HttpContext, nameof(SlideshowManagementController.Upload));
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
                    return ErrorHandler.HandleException(nameof(SlideshowManagementController.Upload), ex, ErrorHandler.UnprocessableStatusCode);
                }

                return Json(new { guid = imageGuid });
            }

            return new HttpStatusCodeResult(ErrorHandler.UnprocessableStatusCode);
        }

        // DELETE: SlideshowManagement/Delete
        [HttpDelete]
        public ActionResult Delete(int pageId, [System.Web.Http.FromBody] Guid? attachmentGuid)
        {
            ErrorHandler.CheckEditMode(HttpContext, nameof(SlideshowManagementController.Delete));

            if (attachmentGuid != null)
            {
                var page = FileManagementHelper.GetPage(pageId);

                if (page != null)
                {
                    var attachment = DocumentHelper.GetAttachment(page, attachmentGuid.Value);

                    if (attachment != null)
                    {
                        try
                        {
                            DocumentHelper.DeleteAttachment(page, attachmentGuid.Value);
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.HandleException(nameof(SlideshowManagementController.Delete), ex, Convert.ToInt32(HttpStatusCode.NoContent));
                        }

                        return new HttpStatusCodeResult(HttpStatusCode.Accepted);
                    } 
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}