using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Web.Mvc;

namespace MedioClinic
{
    public class ApplicationConfig
    {
        public static void RegisterFeatures(IApplicationBuilder builder)
        {
            // Enable required Kentico features

            builder.UsePreview();

            builder.UsePageRouting(new PageRoutingOptions
            {
                EnableAlternativeUrls = true
            });
        }
    }
}