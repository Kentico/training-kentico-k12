using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Business.Services.Errors;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic.Utils
{
    public class ErrorHelper : IErrorHelper
    {
        public int UnprocessableStatusCode => 422;

        protected IErrorHelperService ErrorHelperService { get; }

        public ErrorHelper(IErrorHelperService errorHelperService)
        {
            ErrorHelperService = errorHelperService ?? throw new ArgumentNullException(nameof(errorHelperService));
        }

        public void CheckEditMode(HttpContextBase httpContext, string source)
        {
            if (!httpContext.Kentico().PageBuilder().EditMode)
            {
                throw new HttpException(403, "The operation is only available when the page builder is in the edit mode.");
            }
        }

        public HttpStatusCodeResult HandleException(string source, string eventCode, Exception exception, int statusCode = 500)
        {
            ErrorHelperService.LogException(source, eventCode, exception);
            var flattenedMessage = Regex.Replace(exception.Message, @"\t|\n|\r", " ");

            return new HttpStatusCodeResult(statusCode, flattenedMessage);
        }

        public void LogException(string source, string eventCode, Exception exception) =>
            ErrorHelperService.LogException(source, eventCode, exception);
    }
}