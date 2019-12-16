using System;

namespace Business.Services.Errors
{
    /// <summary>
    /// A common error helper for controller actions
    /// </summary>
    public interface IErrorHelperService : IService
    {
        /// <summary>
        /// Logs an exception to the Kentico event log.
        /// </summary>
        /// <param name="source">The source of the exception to be logged in the Kentico event log.</param>
        /// <param name="exception">The exception to log.</param>
        void LogException(string source, string eventCode, Exception exception);
    }
}