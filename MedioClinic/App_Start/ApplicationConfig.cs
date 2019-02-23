using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic
{
    public class ApplicationConfig
    {
        public static void RegisterFeatures(IApplicationBuilder builder)
        {
            // Enable required Kentico features

            builder.UsePreview();

            builder.UsePageBuilder(new PageBuilderOptions()
            {
                DefaultSectionIdentifier = "MedioClinic.Section.SingleColumn",
                RegisterDefaultSection = true
            });
        }
    }
}