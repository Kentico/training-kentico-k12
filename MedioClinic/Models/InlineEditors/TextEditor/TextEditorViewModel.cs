namespace MedioClinic.Models.InlineEditors
{
    public sealed class TextEditorViewModel : InlineEditorViewModel
    {
        public string Text { get; set; }

        public bool EnableFormatting { get; set; } = true;
    }
}