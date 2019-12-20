using System.Collections.Generic;

namespace MedioClinic.Models
{
    public class MediaLibraryViewModel
    {
        public string LibraryName { get; set; }

        public string LibrarySiteName { get; set; }

        public HashSet<string> AllowedImageExtensions { get; set; }
    }
}