using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.IO;
using CMS.Membership;
using CMS.SiteProvider;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using MedioClinic.Utils;

namespace MedioClinic.Controllers.Widgets
{
    public class ImageUploaderController : Controller
    {
        protected string TempPath => $"{Server.MapPath(@"~\")}App_Data\\Temp\\ImageUploaderEditor";

        protected IFileManagementHelper FileManagementHelper { get; }

        public ImageUploaderController(IFileManagementHelper fileManagementHelper)
        {
            FileManagementHelper = fileManagementHelper ?? throw new ArgumentNullException(nameof(fileManagementHelper));
        }

        [HttpPost]
        public JsonResult Upload(int pageId)
        {
            if (!HttpContext.Kentico().PageBuilder().EditMode)
            {
                throw new HttpException(403, "It is allowed to upload an image only when the page builder is in the edit mode.");
            }

            var page = FileManagementHelper.GetPage(pageId);

            var imageGuid = Guid.Empty;

            foreach (string requestFileName in Request.Files)
            {
                imageGuid = FileManagementHelper.AddUnsortedAttachment(page, requestFileName, Request, TempPath);
            }

            return Json(new { guid = imageGuid });
        }
    }
}