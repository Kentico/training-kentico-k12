using System;
using System.Web;
using System.Web.Mvc;

using CMS.EventLog;
using CMS.SiteProvider;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic.Utils
{
    public class ErrorHelper : IErrorHelper
    {
        public int UnprocessableStatusCode => 422;

        public void CheckEditMode(HttpContextBase httpContext, string source)
        {
            if (!httpContext.Kentico().PageBuilder().EditMode)
            {
                throw new HttpException(403, "The operation is only available when the page builder is in the edit mode.");
            }
        }

        public HttpStatusCodeResult HandleException(string source, string eventCode, Exception exception, int statusCode = 500)
        {
            LogException(source, eventCode, exception);

            return new HttpStatusCodeResult(statusCode);
        }

        public void LogException(string source, string eventCode, Exception exception) =>
            EventLogProvider.LogException(source, eventCode, exception, SiteContext.CurrentSiteID);
    }
}