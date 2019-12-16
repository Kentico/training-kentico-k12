using System;
using System.Web;
using System.Web.Mvc;

namespace MedioClinic.Utils
{
    /// <summary>
    /// A common error helper for controller actions
    /// </summary>
    public interface IErrorHelper
    {
        /// <summary>
        /// HTTP status code of an unprocessable entity.
        /// </summary>
        int UnprocessableStatusCode { get; }

        /// <summary>
        /// Checks if Kentico page builder is in edit mode and throws an <see cref="System.Web.HttpException"/> eventually.
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="source">The source of the exception to be logged in the Kentico event log.</param>
        void CheckEditMode(HttpContextBase httpContext, string source);
        
        /// <summary>
        /// Takes care of logging an exception as well as returning the required status code.
        /// </summary>
        /// <param name="source">The source of the exception to be logged in the Kentico event log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="statusCode">The HTTP status code to return.</param>
        /// <returns>The HTTP status code</returns>
        HttpStatusCodeResult HandleException(string source, string eventCode, Exception exception, int statusCode = 500);

        /// <summary>	
        /// Logs an exception to the Kentico event log.	
        /// </summary>	
        /// <param name="source">The source of the exception to be logged in the Kentico event log.</param>	
        /// <param name="exception">The exception to log.</param>	
        void LogException(string source, string eventCode, Exception exception);
    }
}
