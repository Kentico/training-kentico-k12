using CMS.DocumentEngine;
using System;
using System.Web.Mvc;

namespace MedioClinic.Controllers
{
    public class SlideshowUploaderController : Controller
    {
        // POST: SlideshowUploader/Upload
        [HttpPost]
        public JsonResult Upload(int pageId)
        {
            var page = DocumentHelper.GetDocument(pageId, null);

            //foreach (var fileName in Request.Files)
            //{

            //}

            return Json(new { guid = Guid.NewGuid() });
        }
    }
}