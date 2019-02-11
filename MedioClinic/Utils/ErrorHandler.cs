using CMS.EventLog;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic.Utils
{
    public class ErrorHandler : IErrorHandler
    {
        public int UnprocessableStatusCode => 422;

        public void CheckEditMode(HttpContextBase httpContext, string source)
        {
            if (!httpContext.Kentico().PageBuilder().EditMode)
            {
                throw new HttpException(403, "The operation is only available when the page builder is in the edit mode.");
            }
        }

        public HttpStatusCodeResult HandleException(string source, Exception exception, int statusCode = 500)
        {
            LogException(source, exception);

            return new HttpStatusCodeResult(statusCode);
        }

        public void LogException(string source, Exception exception)
        {
            EventLogProvider.LogException(source, "EXCEPTION", exception, SiteContext.CurrentSiteID);
        }
    }
}