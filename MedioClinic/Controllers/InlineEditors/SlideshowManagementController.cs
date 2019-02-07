using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using MedioClinic.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MedioClinic.Controllers
{
    public class SlideshowManagementController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\SlideshowEditor";

        protected IFileManagementHelper FileManagementHelper { get; }

        public SlideshowManagementController(IFileManagementHelper fileManagementHelper)
        {
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
        }

        // POST: SlideshowManagement/Upload
        [HttpPost]
        public JsonResult Upload(int pageId)
        {
            var page = FileManagementHelper.GetPage(pageId);
            var imageGuid = Guid.Empty;

            foreach (string requestFileName in Request.Files)
            {
                imageGuid = FileManagementHelper.AddUnsortedAttachment(page, requestFileName, Request, TempPath);
            }

            return Json(new { guid = imageGuid });
        }

        // DELETE: SlideshowManagement/Delete
        [HttpDelete]
        public ActionResult Delete(int pageId, [System.Web.Http.FromBody] Guid? attachmentGuid)
        {
            if (attachmentGuid != null)
            {
                var page = FileManagementHelper.GetPage(pageId);
                var attachment = DocumentHelper.GetAttachment(page, attachmentGuid.Value);

                if (attachment != null)
                {
                    try
                    {
                        DocumentHelper.DeleteAttachment(page, attachmentGuid.Value);
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                    }

                    return new HttpStatusCodeResult(HttpStatusCode.Accepted);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}