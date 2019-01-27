using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.DocumentEngine;

namespace MedioClinic.Models.InlineEditors
{
    public class SlideshowEditorViewModel : InlineEditorViewModel
    {
        public IEnumerable<DocumentAttachment> Images { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string SwiperId { get; set; }
    }
}