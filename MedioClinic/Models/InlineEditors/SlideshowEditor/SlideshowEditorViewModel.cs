using System.Collections.Generic;

using CMS.DocumentEngine;

namespace MedioClinic.Models.InlineEditors
{
    public class SlideshowEditorViewModel : InlineEditorViewModel
    {
        public IEnumerable<DocumentAttachment> Images { get; set; }
        public string SwiperId { get; set; }
        public bool EnforceDimensions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}