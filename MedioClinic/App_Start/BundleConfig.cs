using System.Web.Optimization;

namespace MedioClinic
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/master-scripts")
                .IncludeDirectory("~/Scripts/Master", "*.js", true)
            );

            bundles.Add(new StyleBundle("~/bundles/master-css")
                .IncludeDirectory("~/Content/Css/Master", "*.css", true)
            );

            // Enables minification
            BundleTable.EnableOptimizations = true;
        }
    }
}