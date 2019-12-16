using System;
using System.Collections.Generic;

namespace MedioClinic.Config
{
    public static class AppConfig
    {
        public const string Sitename = "MedioClinic";

        public const string MedicalCentersLibrary = "MedicalCenters";

        public static HashSet<string> AllowedImageExtensions =
            new HashSet<string>(new[]
            {
                ".gif",
                ".png",
                ".jpg",
                ".jpeg"
            }, StringComparer.OrdinalIgnoreCase);
    }
}