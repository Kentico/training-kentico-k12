namespace MedioClinic.Models.InlineEditors
{
    public class ImageUploaderEditorViewModel : InlineEditorViewModel
    {
        public bool HasImage { get; set; }

        public MediaLibraryViewModel MediaLibraryViewModel { get; set; }
    }
}