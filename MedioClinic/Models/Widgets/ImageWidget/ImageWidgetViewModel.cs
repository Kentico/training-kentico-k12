namespace MedioClinic.Models.Widgets
{
    public class ImageWidgetViewModel
    {
        public bool HasImage { get; set; }

        public string ImageUrl { get; set; }

        public MediaLibraryViewModel MediaLibraryViewModel { get; set; }
    }
}