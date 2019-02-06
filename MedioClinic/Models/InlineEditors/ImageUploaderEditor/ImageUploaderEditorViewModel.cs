using CMS.DocumentEngine;

namespace MedioClinic.Models.InlineEditors
{
    /// <summary>
    /// View model for Image uploader editor.
    /// </summary>
    public class ImageUploaderEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Image.
        /// </summary>
        public DocumentAttachment Image { get; set; }
    }
}