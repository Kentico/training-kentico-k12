using Business.Services;
using Business.Services.Context;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic.Utils
{
    public class SiteContextService : BaseService, ISiteContextService
    {
        public string SiteName { get; }

        public string CurrentSiteCulture { get; }

        public string PreviewCulture => System.Web.HttpContext.Current.Kentico().Preview().CultureName;

        public bool IsPreviewEnabled => System.Web.HttpContext.Current.Kentico().Preview().Enabled;

        public SiteContextService(string currentCulture, string sitename)
        {
            CurrentSiteCulture = currentCulture;
            SiteName = sitename;
        }
      
    }
}
