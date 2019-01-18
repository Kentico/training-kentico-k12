namespace MedioClinic.Models.InlineEditors
{
    /// <summary>
    /// View model for Text editor.
    /// </summary>
    public sealed class TextEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// HTML formatted text.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Indicates if the formatting is enabled for the editor.
        /// </summary>
        public bool EnableFormatting { get; set; } = true;
    }
}