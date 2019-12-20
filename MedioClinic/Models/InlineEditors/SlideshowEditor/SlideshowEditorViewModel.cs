using System.Collections.Generic;

using Business.Dto.MediaLibrary;

namespace MedioClinic.Models.InlineEditors
{
    public class SlideshowEditorViewModel : InlineEditorViewModel
    {
        public string SwiperId { get; set; }
        public MediaLibraryViewModel MediaLibraryViewModel { get; set; }
        public IEnumerable<MediaLibraryFileDto> Images { get; set; }
    }
}