using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace MvcTrainingMedioClinic
{
    public class ApplicationConfig
    {
        public static void RegisterFeatures(IApplicationBuilder builder)
        {
            // Enable required Kentico features

            builder.UsePreview();
        }
    }
}