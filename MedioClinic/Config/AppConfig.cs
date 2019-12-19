using System;
using System.Collections.Generic;

namespace MedioClinic.Config
{
    public static class AppConfig
    {
        public const string Sitename = "MedioClinic";
        public const string MedicalCentersLibrary = "MedicalCenters";
        public const string ContentDirectory = "~/Content";
        public const string AvatarDirectory = "Avatar";
        public const string DefaultAvatarFileName = "avatar-template.jpg";

        public static HashSet<string> AllowedImageExtensions =
            new HashSet<string>(new[]
            {
                ".gif",
                ".png",
                ".jpg",
                ".jpeg"
            }, StringComparer.OrdinalIgnoreCase);

        public static bool EmailConfirmedRegistration = true;
    }
}