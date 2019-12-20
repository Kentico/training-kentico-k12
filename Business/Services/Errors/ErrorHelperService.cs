using System;

using CMS.EventLog;
using CMS.SiteProvider;

namespace Business.Services.Errors
{
    public class ErrorHelperService : BaseService, IErrorHelperService
    {
        public void LogException(string source, string eventCode, Exception exception) =>
            EventLogProvider.LogException(source, eventCode, exception, SiteContext.CurrentSiteID);
    }
}