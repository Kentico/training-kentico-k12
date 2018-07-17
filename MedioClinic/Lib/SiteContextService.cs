using Kentico.Content.Web.Mvc;
using Kentico.Services.Context;
using Kentico.Web.Mvc;

namespace MedioClinic
{
    public class SiteContext : ISiteContextService
    {
        public string CurrentSiteCulture { get; }

        public string PreviewCulture => System.Web.HttpContext.Current.Kentico().Preview().CultureName;

        public bool IsPreviewEnabled => System.Web.HttpContext.Current.Kentico().Preview().Enabled;

        public SiteContext(string currentCulture)
        {
            CurrentSiteCulture = currentCulture;
        }
      
    }
}
