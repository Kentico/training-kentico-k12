using Kentico.Content.Web.Mvc;
using Kentico.Services.Context;
using Kentico.Web.Mvc;

namespace MedioClinic
{
    public class SiteContext : ISiteContext
    {
        private string ActiveCulture { get; } 

        public SiteContext(string activeCulture)
        {
            ActiveCulture = activeCulture;
        }

        public string GetActiveSiteCulture()
        {
            return ActiveCulture;
        }

        public string GetPreviewCulture()
        {
            return System.Web.HttpContext.Current.Kentico().Preview().CultureName;
        }

        public bool IsPreviewEnabled()
        {
            return System.Web.HttpContext.Current.Kentico().Preview().Enabled;
        }
    }
}
